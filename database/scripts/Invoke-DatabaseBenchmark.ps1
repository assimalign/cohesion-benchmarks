[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('sql','documents','graph')][string]$Model,
    [Parameter(Mandatory)][string[]]$Targets,
    [Parameter(Mandatory)][ValidateRange(0.000001, 1000)][double]$Scale,
    [ValidateSet('latency','throughput','mixed')][string[]]$Suites = @('latency','throughput','mixed'),
    [string]$Tag = 'dev',
    [string]$Suffix = '',
    [string]$OutputDirectory,
    [ValidateRange(1,100)][int]$Repeat = 3
)
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
if (-not $OutputDirectory) { $OutputDirectory = Join-Path $script:DatabaseRoot ('artifacts/results/' + [DateTime]::UtcNow.ToString('yyyyMMddTHHmmss')) }
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
$datasetVolume = "dbbench-datasets$Suffix"
& podman network exists dbbench
if ($LASTEXITCODE -ne 0) { Invoke-Podman @('network','create','dbbench') | Out-Null }
& podman volume exists $datasetVolume
if ($LASTEXITCODE -ne 0) { Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",$datasetVolume) | Out-Null }
$image = "localhost/dbbench-driver:$Tag"
$scaleText = $Scale.ToString('0.################', [Globalization.CultureInfo]::InvariantCulture)
$data = "/data/$Model/sf$scaleText"
$script:activeEngine = $null
function Invoke-Driver {
    param([string[]]$Command, [hashtable]$Engine)
    $driverName = "dbbench-driver$Suffix"
    $memory = if ($Engine -and $Engine.kind -eq 'embedded') { '6g' } else { '3g' }
    $arguments = @('run','--rm','--name',$driverName,'--network','dbbench','--cpus','3','--memory',$memory,'--memory-swap',$memory,
        '--volume',"${datasetVolume}:/data",'--volume',"${OutputDirectory}:/results")
    if ($Engine) { $arguments += @('--volume',"$($Engine.volume)$Suffix`:/engine-data") }
    $job = $null
    if ($Engine -and $Command[0] -in @('load','run')) {
        $sampledContainer = if ($Engine.kind -eq 'embedded') { $driverName } else { $Engine.containerName + $Suffix }
        $telemetryPath = Join-Path $OutputDirectory 'host-telemetry.json'
        $job = Start-ThreadJob -FilePath (Join-Path $PSScriptRoot 'Measure-BenchHost.ps1') -ArgumentList $sampledContainer,$telemetryPath
        $arguments += @('--env','DBBENCH_HOST_TELEMETRY=/results/host-telemetry.json')
    }
    try { Invoke-Podman ($arguments + @($image) + $Command) }
    finally {
        if ($job) { Stop-Job $job; Receive-Job $job; Remove-Job $job }
        & podman container exists $driverName
        if ($LASTEXITCODE -eq 0) { & podman stop --time 40 $driverName | Out-Null; & podman rm $driverName | Out-Null }
    }
}
# Only data files use named volumes. The result/telemetry directory is a host artifact bind.
& podman run --rm --name "dbbench-dataset-check$Suffix" --volume "${datasetVolume}:/data" --entrypoint /bin/sh $image -c "test -f '$data/manifest.json'"
if ($LASTEXITCODE -ne 0) { Invoke-Driver @('generate','--model',$Model,'--scale',$scaleText,'--out','/data') }
$loaded = @{}
foreach ($id in $Targets) {
    $engine = Get-BenchTarget $id $Suffix
    if ($engine.model -ne $Model) { throw "$id belongs to a different model." }
    # TCP serves the embedded model's files; never let both processes open that volume at once.
    if ($engine.ContainsKey('servesLoadedDataFrom') -and -not $loaded.ContainsKey($engine.servesLoadedDataFrom)) {
        $embedded = Get-BenchTarget $engine.servesLoadedDataFrom $Suffix
        & podman volume exists ($embedded.volume + $Suffix)
        if ($LASTEXITCODE -ne 0) { Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",($embedded.volume + $Suffix)) | Out-Null }
        Invoke-Driver @('load','--model',$Model,'--target',$embedded.id,'--data',$data,'--official','--out','/results') $embedded
        $loaded[$embedded.id] = $true
    }
    try {
        if ($engine.kind -eq 'container') { & (Join-Path $PSScriptRoot 'Start-BenchEngine.ps1') -Target $id -Suffix $Suffix -Tag $Tag | Write-Host }
        else {
            & podman volume exists ($engine.volume + $Suffix)
            if ($LASTEXITCODE -ne 0) { Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",($engine.volume + $Suffix)) | Out-Null }
        }
        $common = @('--model',$Model,'--target',$id,'--data',$data,'--suffix',$Suffix)
        if (-not $Suffix) { $common = @('--model',$Model,'--target',$id,'--data',$data) }
        # Preserve engine-side image digest and limits even though the driver does not contain Podman.
        if ($engine.kind -eq 'container') {
            $raw = Invoke-Podman @('inspect',($engine.containerName + $Suffix)) | Out-String
            $inspection = @($raw | ConvertFrom-Json -AsHashtable)[0]
            @{schemaVersion=1;container=($engine.containerName+$Suffix);containerInspectJson=$raw;imageDigest=$inspection.ImageDigest} |
                ConvertTo-Json -Depth 20 | Set-Content -LiteralPath (Join-Path $OutputDirectory "$id.environment.json")
        }
        $environmentArgs = if ($engine.kind -eq 'container') { @('--environment',"/results/$id.environment.json") } else { @() }
        Invoke-Driver (@('load') + $common + @('--official','--out','/results') + $environmentArgs) $engine
        $loaded[$id] = $true
        Invoke-Driver (@('verify') + $common + @('--out',"/results/$id.verify.json")) $engine
        foreach ($suite in $Suites) {
            $mixes = if ($suite -eq 'mixed') { @('read-heavy','balanced') } else { @('read-heavy') }
            foreach ($mix in $mixes) {
                Invoke-Driver (@('run') + $common + @('--suite',$suite,'--mix',$mix,'--repeat',[string]$Repeat,
                    '--verification',"/results/$id.verify.json",'--official','--out','/results') + $environmentArgs) $engine
            }
        }
    } finally {
        if ($engine.kind -eq 'container') { & (Join-Path $PSScriptRoot 'Stop-BenchEngine.ps1') -Target $id -Suffix $Suffix }
    }
}
# Report always runs, even when comparison reports a blocking mismatch.
$comparisonFailed = $false
try { if ($Targets.Count -gt 1) { Invoke-Driver @('verify','--compare','/results','--out','/results/mismatches.json') } }
catch { $comparisonFailed = $true; Write-Warning $_ }
Invoke-Driver @('report','--in','/results','--out','/results/report')
if ($comparisonFailed) { throw 'Verification mismatches block performance reporting; see report and mismatches.json.' }
