[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('sql','documents','graph')][string]$Model,
    [string[]]$Targets,
    [ValidateRange(0.000001,1000)][double]$Scale = 10,
    [ValidateSet('load','verify','read-latency','read-throughput','write-latency','write-throughput','mixed','compare','report')]
    [string[]]$Phases = @('load','verify','read-latency','read-throughput','write-latency','write-throughput','mixed','compare','report'),
    [switch]$SkipLoad,
    [string]$DatasetVolume = 'dbbench-datasets',
    [string]$Tag = 'dev',
    [string]$Suffix = '',
    [ValidatePattern('^[a-zA-Z0-9_.-]+$')][string]$RunId = [DateTime]::UtcNow.ToString('yyyyMMddTHHmmssZ'),
    [string]$OutputDirectory,
    [ValidateRange(1,100)][int]$Repeat = 1,
    [ValidateRange(0,86400)][double]$WarmupSeconds = 10,
    [ValidateRange(0.001,86400)][double]$DurationSeconds = 30,
    [ValidateRange(1,2147483647)][int]$PointIterations = 5000,
    [ValidateRange(0,1000)][int]$AnalyticsWarmup = 1,
    [ValidateRange(1,10000)][int]$AnalyticsIterations = 3,
    [ValidateRange(1,1024)][int[]]$Clients = @(1,8,32),
    [ValidateRange(1,1024)][int[]]$MixedClients = @(8,32),
    [ValidateRange(0.001,86400)][double]$PointTimeoutSeconds = 30,
    [ValidateRange(0.001,86400)][double]$AnalyticsTimeoutSeconds = 600,
    [string[]]$Operations
)
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
if (-not $OutputDirectory) { $OutputDirectory = Join-Path $script:DatabaseRoot "artifacts/results/$RunId" }
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
$artifactRoot = [IO.Path]::GetFullPath((Join-Path $script:DatabaseRoot 'artifacts')) + [IO.Path]::DirectorySeparatorChar
if (-not $OutputDirectory.StartsWith($artifactRoot, [StringComparison]::OrdinalIgnoreCase)) { throw 'Results must be inside database/artifacts.' }
if ($DatasetVolume -notmatch '^dbbench-[a-z0-9-]+$') { throw 'Invalid dataset volume name.' }
$null = Get-BenchTarget (@($script:Catalog.targets | Where-Object model -eq $Model)[0].id) $Suffix
$modelDirectory = Join-Path $OutputDirectory $Model
New-Item -ItemType Directory -Path $modelDirectory -Force | Out-Null
$log = Join-Path $modelDirectory 'orchestration.log'
$ledger = Join-Path $modelDirectory 'phases.jsonl'
$image = "localhost/dbbench-driver:$Tag"
$driverName = "dbbench-driver$Suffix"
$script:Failures = 0
$script:ActiveEngine = $null
$script:PhaseDirectory = $modelDirectory
$script:Plan = $null
$scaleText = $Scale.ToString('0.################', [Globalization.CultureInfo]::InvariantCulture)
$data = "/data/$Model/sf$scaleText"
function Number([double]$Value) { $Value.ToString('0.################', [Globalization.CultureInfo]::InvariantCulture) }
function Write-RunLog([string]$Message) {
    $line = "$([DateTimeOffset]::UtcNow.ToString('O')) $Message"
    $line | Add-Content -LiteralPath $log
    Write-Host $line
}
function Archive-Phase([string]$Path, [string]$TargetId, [string]$Phase) {
    if (-not (Test-Path -LiteralPath $Path)) { return }
    $destination = [IO.Path]::GetFullPath((Join-Path $OutputDirectory "_history/$Model/$TargetId/$Phase/$([DateTime]::UtcNow.ToString('yyyyMMddTHHmmssfffffff'))"))
    foreach ($candidate in @([IO.Path]::GetFullPath($Path), $destination)) {
        if (-not $candidate.StartsWith($OutputDirectory + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) { throw 'Archive path escaped result root.' }
    }
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Move-Item -LiteralPath $Path -Destination $destination
}
function Invoke-Phase([string]$TargetId, [string]$Phase, [scriptblock]$Action) {
    $script:PhaseDirectory = Join-Path $modelDirectory "$TargetId/$Phase"
    Archive-Phase $script:PhaseDirectory $TargetId $Phase
    New-Item -ItemType Directory -Path $script:PhaseDirectory -Force | Out-Null
    $started = [DateTimeOffset]::UtcNow
    $watch = [Diagnostics.Stopwatch]::StartNew()
    Write-RunLog "START $TargetId $Phase"
    $failure = $null
    try { & $Action *>&1 | Tee-Object -FilePath (Join-Path $script:PhaseDirectory 'console.log') | ForEach-Object { Write-Host $_ } }
    catch { $failure = $_.Exception.Message; $script:Failures++; Write-RunLog "ERROR $TargetId $Phase $failure" }
    $watch.Stop()
    $status = if ($failure) { 'failed' } else { 'passed' }
    $record = @{target=$TargetId;phase=$Phase;status=$status;startedUtc=$started.ToString('O');finishedUtc=[DateTimeOffset]::UtcNow.ToString('O');seconds=$watch.Elapsed.TotalSeconds;error=$failure}
    $record | ConvertTo-Json -Compress | Add-Content -LiteralPath $ledger
    $record | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $script:PhaseDirectory 'phase.status.json')
    Write-RunLog "END $TargetId $Phase status=$status duration=$($watch.Elapsed.TotalSeconds.ToString('F3', [Globalization.CultureInfo]::InvariantCulture))s"
    return -not $failure
}
function Ensure-Volume([string]$Name) {
    & podman volume exists $Name
    if ($LASTEXITCODE -eq 0) { Assert-BenchOwner $Name -Volume }
    else { Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",$Name) | Out-Null }
}
function Remove-OwnedVolume([string]$Name) {
    & podman volume exists $Name
    if ($LASTEXITCODE -eq 0) { Assert-BenchOwner $Name -Volume; Invoke-Podman @('volume','rm',$Name) | Out-Null }
}
function Stop-Engine {
    if ($script:ActiveEngine) {
        & (Join-Path $PSScriptRoot 'Stop-BenchEngine.ps1') -Target $script:ActiveEngine -Suffix $Suffix
        $script:ActiveEngine = $null
    }
}
function Start-Engine([hashtable]$Engine, [string]$Volume) {
    Stop-Engine
    if ($Engine.kind -eq 'container') {
        & (Join-Path $PSScriptRoot 'Start-BenchEngine.ps1') -Target $Engine.id -Suffix $Suffix -Tag $Tag -DataVolume $Volume -Cpus 3 -Memory 6g -TimeoutSeconds 600
        $script:ActiveEngine = $Engine.id
    }
}
function Copy-Volume([string]$Source, [string]$Destination) {
    Stop-Engine
    Assert-BenchOwner $Source -Volume
    Remove-OwnedVolume $Destination
    Ensure-Volume $Destination
    Write-RunLog "COPY $Source -> $Destination (offline; no database statements)"
    Invoke-Podman @('run','--rm','--name',"dbbench-copy$Suffix",'--label',"dbbench.owner=$script:Owner",'--network','none',
        '--cpus','3','--memory','512m','--memory-swap','512m','--volume',"${Source}:/from:ro",'--volume',"${Destination}:/to",
        '--entrypoint','/bin/sh',$image,'-c','cp -a /from/. /to/')
}
function Container-Path([string]$Path) { '/results/' + [IO.Path]::GetRelativePath($OutputDirectory, $Path).Replace('\','/') }
function Invoke-Driver([string[]]$Command, [hashtable]$Engine, [string]$Volume, [switch]$Import) {
    & podman container exists $driverName
    if ($LASTEXITCODE -eq 0) { throw "$driverName already exists; refusing to interrupt it." }
    $memory = if ($Engine -and $Engine.kind -eq 'embedded') { '6g' } else { '3g' }
    $arguments = @('run','--rm','--name',$driverName,'--label',"dbbench.owner=$script:Owner",'--network','dbbench',
        '--cpus','3','--memory',$memory,'--memory-swap',$memory,'--volume',"${DatasetVolume}:/data:ro",'--volume',"${OutputDirectory}:/results")
    $jobs = @()
    $inputVolume = "dbbench-neo4j-import-input$Suffix"
    if ($Engine) {
        $mode = if ($Engine.kind -eq 'embedded') { '' } else { ':ro' }
        $arguments += @('--volume',"${Volume}:/engine-data$mode")
        if ($Engine.kind -eq 'container' -and $Command[0] -in @('load','run')) {
            $raw = Invoke-Podman @('inspect',($Engine.containerName + $Suffix)) | Out-String
            $inspect = @($raw | ConvertFrom-Json -AsHashtable)[0]
            $environmentFile = Join-Path $script:PhaseDirectory 'environment.json'
            @{schemaVersion=1;containerInspectJson=$raw;imageDigest=$inspect.ImageDigest} | ConvertTo-Json -Depth 30 | Set-Content -LiteralPath $environmentFile
            $Command += @('--environment',(Container-Path $environmentFile))
        }
        if ($Command[0] -in @('load','run')) {
            $sampled = if ($Engine.kind -eq 'embedded') { $driverName } else { $Engine.containerName + $Suffix }
            $telemetry = Join-Path $script:PhaseDirectory 'host.telemetry'
            $jobs += Start-ThreadJob -FilePath (Join-Path $PSScriptRoot 'Measure-BenchHost.ps1') -ArgumentList $sampled,$telemetry
            $arguments += @('--env',('DBBENCH_HOST_TELEMETRY=' + (Container-Path $telemetry)))
        }
    }
    if ($Import) {
        Remove-OwnedVolume $inputVolume
        Ensure-Volume $inputVolume
        $arguments += @('--volume',"${inputVolume}:/import",'--env','DBBENCH_IMPORT_DIRECTORY=/import',
            '--env',('DBBENCH_IMPORT_CONTROL=' + (Container-Path $script:PhaseDirectory)))
        $jobs += Start-ThreadJob -FilePath (Join-Path $PSScriptRoot 'Watch-Neo4jImport.ps1') -ArgumentList ($Engine.containerName + $Suffix),$inputVolume,$script:PhaseDirectory,$Suffix
    }
    Write-RunLog "COMMAND $driverName memory=$memory dbbench $($Command -join ' ')"
    try { Invoke-Podman ($arguments + @($image) + $Command) }
    finally {
        foreach ($job in $jobs) { Stop-Job $job; Receive-Job $job; Remove-Job $job }
        & podman container exists $driverName
        if ($LASTEXITCODE -eq 0) {
            Assert-BenchOwner $driverName
            Invoke-Podman @('stop','--time','40',$driverName) | Out-Null
            Invoke-Podman @('rm',$driverName) | Out-Null
        }
        if ($Import) {
            $helper = "dbbench-neo4j-import$Suffix"
            & podman container exists $helper
            if ($LASTEXITCODE -eq 0) { Assert-BenchOwner $helper; Invoke-Podman @('rm','-f',$helper) | Out-Null }
            Remove-OwnedVolume $inputVolume
        }
    }
}
function Common-Arguments([string]$Id) {
    $values = @('--model',$Model,'--target',$Id,'--data',$data)
    if ($Suffix) { $values += @('--suffix',$Suffix) }
    return $values
}
function Load-Target([hashtable]$Engine) {
    $volume = $Engine.volume + $Suffix
    $checkpoint = $Engine.volume + '-pristine' + $Suffix
    $receipt = Join-Path $modelDirectory "_state/$($Engine.id).checkpoint.json"
    Archive-Phase $receipt $Engine.id 'checkpoint'
    Stop-Engine
    Remove-OwnedVolume $volume
    Ensure-Volume $volume
    Start-Engine $Engine $volume
    Invoke-Driver (@('load') + (Common-Arguments $Engine.id) + @('--official','--out',(Container-Path $script:PhaseDirectory))) $Engine $volume -Import:($Engine.id -eq 'neo4j')
    Copy-Volume $volume $checkpoint
    New-Item -ItemType Directory -Path (Split-Path $receipt -Parent) -Force | Out-Null
    @{model=$Model;scale=$Scale;manifestSha256=$script:ManifestHash;volume=$checkpoint;image=$image} | ConvertTo-Json | Set-Content -LiteralPath $receipt
}
& podman network exists dbbench
if ($LASTEXITCODE -ne 0) { Invoke-Podman @('network','create','--label','dbbench.shared=true','dbbench') | Out-Null }
$ready = Invoke-Phase '_model' 'dataset' {
    & podman volume exists $DatasetVolume
    if ($LASTEXITCODE -ne 0) { throw "Dataset volume missing: $DatasetVolume. Generate it explicitly before running." }
    $hash = Invoke-Podman @('run','--rm','--name',"dbbench-dataset-check$Suffix",'--network','none','--cpus','3','--memory','256m','--memory-swap','256m',
        '--volume',"${DatasetVolume}:/data:ro",'--entrypoint','sha256sum',$image,"$data/manifest.json")
    $script:ManifestHash = ($hash -split ' ')[0]
    if ($script:ManifestHash -notmatch '^[a-f0-9]{64}$') { throw 'Invalid manifest hash.' }
    Write-Output "Dataset $data manifest=$script:ManifestHash (read-only)"
}
if ($ready) {
    $null = Invoke-Phase '_model' 'plan' {
        $json = Invoke-Driver @('list','--model',$Model,'--json')
        $script:Plan = ($json | Out-String) | ConvertFrom-Json -AsHashtable
        $json | Set-Content -LiteralPath (Join-Path $script:PhaseDirectory 'plan.json')
    }
}
$plan = $script:Plan
if ($plan) {
    if (-not $Targets) { $Targets = @($plan.targets | ForEach-Object id) }
    foreach ($id in $Targets) { if ($id -notin @($plan.targets | ForEach-Object id)) { throw "Target $id is not registered for $Model." } }
    foreach ($op in $Operations) { if ($op -notin @($plan.operations | ForEach-Object id)) { throw "Unknown operation: $op" } }
    $Targets = @($plan.targets | Where-Object { $_.id -in $Targets } | ForEach-Object id)
    $selected = @($plan.operations | Where-Object { -not $Operations -or $_.id -in $Operations })
    $mixed = @($selected | Where-Object {
        $operation = $_.id
        ($_.readHeavyWeight -gt 0 -or $_.balancedWeight -gt 0) -and
        @($plan.targets | Where-Object { $plan.support[$_.id][$operation].level -eq 'Unsupported' }).Count -eq 0
    } | ForEach-Object id)
    Write-RunLog "MIX comparable subset across every model target: $($mixed -join ','); descriptor weights within each class, 90/10 or 50/50 read/write"
    $timeArgs = @('--warmup',((Number $WarmupSeconds)+'s'),'--duration',((Number $DurationSeconds)+'s'),
        '--repeat',[string]$Repeat,'--analytics-warmup',[string]$AnalyticsWarmup,'--analytics-iterations',[string]$AnalyticsIterations,
        '--point-timeout',((Number $PointTimeoutSeconds)+'s'),'--analytics-timeout',((Number $AnalyticsTimeoutSeconds)+'s'))
    foreach ($id in $Targets) {
        $engine = Get-BenchTarget $id $Suffix
        $volume = $engine.volume + $Suffix
        $checkpoint = $engine.volume + '-pristine' + $Suffix
        $verifyVolume = $engine.volume + '-verify' + $Suffix
        $targetDirectory = Join-Path $modelDirectory $id
        New-Item -ItemType Directory -Path $targetDirectory -Force | Out-Null
        $verification = Join-Path $targetDirectory "$id.verify.json"
        $common = Common-Arguments $id
        $activePhases = @($Phases | Where-Object { $_ -notin @('compare','report') -and (-not $SkipLoad -or $_ -ne 'load') })
        if ($activePhases.Count -eq 0) { continue }
        try {
            $dataOwner = if ($engine.ContainsKey('servesLoadedDataFrom')) { $engine.servesLoadedDataFrom } else { $id }
            if ('load' -in $activePhases -and $dataOwner -eq $id) {
                $null = Invoke-Phase $id 'load' { Load-Target $engine }
            } elseif ('load' -in $activePhases -and $dataOwner -notin $Targets) {
                $embedded = Get-BenchTarget $dataOwner $Suffix
                $null = Invoke-Phase $dataOwner 'load' { Load-Target $embedded }
            }
            $dataReady = Invoke-Phase $id 'start' {
                $receiptPath = Join-Path $modelDirectory "_state/$dataOwner.checkpoint.json"
                if (-not (Test-Path -LiteralPath $receiptPath)) { throw "No successful load checkpoint for $dataOwner; include load or reuse its run directory." }
                $receipt = Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json
                if ($receipt.manifestSha256 -ne $script:ManifestHash) { throw 'Checkpoint belongs to a different dataset.' }
                Copy-Volume $checkpoint $volume
                Start-Engine $engine $volume
            }
            if ('load' -in $activePhases -and $dataOwner -ne $id) {
                $null = Invoke-Phase $id 'load' {
                    if (-not $dataReady) { throw 'Start/checkpoint failed; cannot run against an unknown store.' }
                    Invoke-Driver (@('load') + $common + @('--official','--out',(Container-Path $script:PhaseDirectory))) $engine $volume
                    Start-Engine $engine $volume
                }
            }
            if ('verify' -in $activePhases) {
                $null = Invoke-Phase $id 'verify' {
                    if (-not $dataReady) { throw 'Start/checkpoint failed; cannot verify an unknown store.' }
                    Copy-Volume $checkpoint $verifyVolume
                    Start-Engine $engine $verifyVolume
                    Archive-Phase $verification $id 'verification'
                    try { Invoke-Driver (@('verify') + $common + @('--out',(Container-Path $verification))) $engine $verifyVolume }
                    finally { Stop-Engine; Remove-OwnedVolume $verifyVolume; Start-Engine $engine $volume }
                }
            }
            foreach ($access in @('Read','Write')) {
                $opIds = @($selected | Where-Object access -eq $access | ForEach-Object id)
                if ($opIds.Count -eq 0) { continue }
                foreach ($suite in @('latency','throughput')) {
                    $phase = $access.ToLowerInvariant() + '-' + $suite
                    if ($phase -notin $activePhases) { continue }
                    $null = Invoke-Phase $id $phase {
                        if (-not $dataReady) { throw 'Start/checkpoint failed; cannot measure an unknown store.' }
                        $limits = if ($suite -eq 'latency') { @('--iterations',[string]$PointIterations) } else { @('--clients',($Clients -join ',')) }
                        Invoke-Driver (@('run') + $common + @('--suite',$suite,'--ops',($opIds -join ','),'--verification',(Container-Path $verification),
                            '--official','--out',(Container-Path $script:PhaseDirectory)) + $timeArgs + $limits) $engine $volume
                    }
                }
            }
            if ('mixed' -in $activePhases) {
                foreach ($mix in @('read-heavy','balanced')) {
                    $null = Invoke-Phase $id "mixed-$mix" {
                        if (-not $dataReady) { throw 'Start/checkpoint failed; cannot measure an unknown store.' }
                        if ($mixed.Count -eq 0) { throw 'No common supported weighted operations.' }
                        Invoke-Driver (@('run') + $common + @('--suite','mixed','--mix',$mix,'--clients',($MixedClients -join ','),
                            '--ops',($mixed -join ','),'--verification',(Container-Path $verification),'--official','--out',(Container-Path $script:PhaseDirectory)) + $timeArgs) $engine $volume
                    }
                }
            }
        } finally { $null = Invoke-Phase $id 'stop' { Stop-Engine } }
    }
    if ('compare' -in $Phases) { $null = Invoke-Phase '_model' 'compare' { Invoke-Driver @('verify','--compare',(Container-Path $modelDirectory),'--out',((Container-Path $script:PhaseDirectory)+'/mismatches.json')) } }
    if ('report' -in $Phases) { $null = Invoke-Phase '_model' 'report' { Invoke-Driver @('report','--in',(Container-Path $modelDirectory),'--out',(Container-Path $script:PhaseDirectory)) } }
}
Write-RunLog "MODEL END $Model failedPhases=$script:Failures results=$modelDirectory"
if ($script:Failures) { exit 2 }
