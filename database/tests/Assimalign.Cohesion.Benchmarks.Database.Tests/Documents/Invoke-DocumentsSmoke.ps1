[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('cohesion-documents-embedded','mongodb','ravendb','cosmos-emulator')][string]$Target,
    [string]$Root = 'database/artifacts/documents-r3-smoke',
    [string]$Suffix = '-r3',
    [switch]$Driver,
    [switch]$VerifyOnly,
    [int]$WaitSeconds = 1800
)
$ErrorActionPreference = 'Stop'
$workspace = (Resolve-Path (Join-Path $PSScriptRoot '../../../..')).Path
Push-Location $workspace
$createdEngine = $false
$createdVolumes = [Collections.Generic.List[string]]::new()
try {
    $rootPath = [IO.Path]::GetFullPath($Root)
    if (-not $rootPath.StartsWith($workspace + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Smoke artifacts must stay inside this benchmarks worktree.'
    }
    $mode = if ($Driver) { 'container' } else { 'windows' }
    $outputPath = Join-Path $rootPath $mode
    New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
    $logPath = Join-Path $outputPath "$Target.commands.log"
    $cli = 'database/artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
    function Invoke-Checked([string]$Executable, [string[]]$Arguments) {
        "$Executable $($Arguments -join ' ')" | Tee-Object -FilePath $logPath -Append | Write-Host
        & $Executable @Arguments 2>&1 | Tee-Object -FilePath $logPath -Append | Write-Host
        if ($LASTEXITCODE -ne 0) { throw "$Executable exited $LASTEXITCODE" }
    }
    $deadline = [DateTime]::UtcNow.AddSeconds($WaitSeconds)
    $previousWait = ''
    do {
        $allRunning = @(& podman ps --format '{{.Names}}')
        if ($LASTEXITCODE -ne 0) { throw 'Cannot inspect the Podman engine slot.' }
        $ownedNames = @('dbbench-mongodb', 'dbbench-ravendb', 'dbbench-cosmos-emulator', 'dbbench-driver-documents') | ForEach-Object { $_ + $Suffix }
        $running = @($allRunning | Where-Object { $_ -in $ownedNames })
        if ($allRunning.Count -gt 0 -and $previousWait -eq '') {
            "Other session containers observed: $($allRunning -join ', ')" | Add-Content -LiteralPath $logPath
        }
        if ($running.Count -eq 0) { break }
        if ([DateTime]::UtcNow -gt $deadline) { throw "Other benchmark engine is still active: $running" }
        if (($running -join ',') -ne $previousWait) { Write-Host "WAIT engine slot: $running"; $previousWait = $running -join ',' }
        Start-Sleep -Seconds 10
    } while ($true)
    if ($Driver) {
        if ($Target -ne 'cohesion-documents-embedded') { throw 'Driver mode in this smoke script is the embedded resource-budget proof.' }
        $canonicalManifest = Join-Path $rootPath 'data/documents/sf0.01/manifest.json'
        if (-not (Test-Path -LiteralPath $canonicalManifest)) {
            Invoke-Checked dotnet @($cli,'generate','--model','documents','--scale','0.01','--threads','2','--out',(Join-Path $rootPath 'data'))
        }
        & podman container exists ('dbbench-driver-documents' + $Suffix)
        if ($LASTEXITCODE -eq 0) { throw 'Refusing a pre-existing driver container.' }
        $dataVolume = 'dbbench-documents-datasets' + $Suffix
        $engineVolume = 'dbbench-cohesion-documents-data' + $Suffix
        foreach ($volume in @($dataVolume, $engineVolume)) {
            & podman volume exists $volume
            if ($LASTEXITCODE -eq 0) { throw "Refusing pre-existing volume $volume; this invocation must own its data." }
            Invoke-Checked podman @('volume','create','--label',"dbbench.owner=$workspace",$volume)
            $createdVolumes.Add($volume)
        }
        $driverArgs = @('run','--rm','--name',('dbbench-driver-documents' + $Suffix),'--network','dbbench',
            '--cpus','3','--memory','6g','--memory-swap','6g','--volume',"${dataVolume}:/data",'--volume',"${engineVolume}:/engine-data",'--volume',"${outputPath}:/results",'localhost/dbbench-driver:r3')
        function Invoke-Bench([string[]]$Arguments) { Invoke-Checked podman ($driverArgs + $Arguments) }
        Invoke-Bench @('generate','--model','documents','--scale','0.01','--threads','2','--out','/data')
        # The foundation serializer uses platform newlines in manifests. Assert identical metadata and
        # generated part hashes before distributing the exact same manifest bytes to every target.
        $manifestCommand = @'
set -eu
tr -d '\r' < /canonical-manifest.json | cmp - /data/documents/sf0.01/manifest.json
cp /canonical-manifest.json /data/documents/sf0.01/manifest.json
'@
        $manifestCommand = $manifestCommand.Replace("`r`n", "`n")
        $manifestArgs = $driverArgs[0..($driverArgs.Length - 2)] + @('--volume',"${canonicalManifest}:/canonical-manifest.json:ro",'--entrypoint','/bin/sh',$driverArgs[-1],'-c',$manifestCommand)
        Invoke-Checked podman $manifestArgs
        $dataPath = '/data/documents/sf0.01'; $resultPath = '/results'; $storagePath = '/engine-data'
    } else {
        function Invoke-Bench([string[]]$Arguments) { Invoke-Checked dotnet (@($cli) + $Arguments) }
        $dataPath = Join-Path $rootPath 'data/documents/sf0.01'
        if (-not (Test-Path (Join-Path $dataPath 'manifest.json'))) {
            Invoke-Bench @('generate','--model','documents','--scale','0.01','--threads','2','--out',(Join-Path $rootPath 'data'))
        }
        $resultPath = $outputPath; $storagePath = Join-Path $rootPath 'cohesion-windows'
        if ($Target -ne 'cohesion-documents-embedded') {
            foreach ($resource in @(@('container', "dbbench-$Target$Suffix"), @('volume', "dbbench-$Target-data$Suffix"))) {
                & podman $resource[0] exists $resource[1]
                if ($LASTEXITCODE -eq 0) { throw "Refusing pre-existing $($resource[0]) $($resource[1])." }
            }
            $memory = if ($Target -eq 'cosmos-emulator') { '3g' } else { '2g' }
            $createdEngine = $true
            & ./database/scripts/Start-BenchEngine.ps1 -Target $Target -Memory $memory -Suffix $Suffix
        }
    }
    $common = @('--model','documents','--target',$Target,'--data',$dataPath,'--database','benchmark_r3','--storage',$storagePath,'--suffix',$Suffix)
    Invoke-Bench (@('load') + $common + @('--out',$resultPath))
    $verification = "$resultPath/$Target.verify.json"
    Invoke-Bench (@('verify') + $common + @('--out',$verification))
    if (-not $VerifyOnly) {
        Invoke-Bench (@('run') + $common + @('--suite','latency','--duration','10s','--iterations','20','--warmup','1s','--repeat','1','--verification',$verification,'--out',$resultPath))
        Invoke-Bench (@('run') + $common + @('--suite','throughput','--clients','1,8','--duration','10s','--warmup','1s','--repeat','1','--verification',$verification,'--out',$resultPath))
    }
    if (@(Get-ChildItem -LiteralPath $outputPath -Filter '*.verify.json').Count -ge 2) {
        Invoke-Checked dotnet @($cli,'verify','--compare',$outputPath,'--out',(Join-Path $outputPath 'comparison.json'))
    }
    Invoke-Checked dotnet @($cli,'report','--in',$outputPath,'--out',(Join-Path $outputPath 'report'))
} finally {
    if ($createdEngine) { & ./database/scripts/Stop-BenchEngine.ps1 -Target $Target -Suffix $Suffix -RemoveData }
    if ($Driver) {
        $driverName = 'dbbench-driver-documents' + $Suffix
        # The name is used only after this invocation has successfully created its private volumes.
        if ($createdVolumes.Count -gt 0) {
            & podman container exists $driverName
            if ($LASTEXITCODE -eq 0) { & podman stop --time 40 $driverName; & podman rm $driverName }
        }
        foreach ($volume in $createdVolumes) { & podman volume rm $volume }
    }
    Pop-Location
}
