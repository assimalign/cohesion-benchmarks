[CmdletBinding()]
param(
    [string]$Tag = 'r5',
    [string]$Suffix = '-r5',
    [string]$DatasetVolume = 'dbbench-datasets-r5',
    [string]$PristineVolume = 'dbbench-graph-diagnostic-pristine-r5',
    [string]$ToolsVolume = 'dbbench-diagnostics-tools-r5',
    [string]$SdkImage = 'mcr.microsoft.com/dotnet/sdk:10.0.300-noble-aot',
    [double]$Scale = 0.01,
    [string]$Memory = '2g',
    [int]$BufferPoolMb = 1024,
    [int[]]$Clients = @(1,8),
    [double]$WarmupSeconds = 1,
    [double]$DurationSeconds = 10,
    [string[]]$Operations,
    [int[]]$DumpAtSeconds = @(),
    [ValidateRange(30,3600)][int]$MaximumSeconds = 900,
    [Parameter(Mandatory)][string]$OutputDirectory
)
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
$null = Get-BenchTarget 'cohesion-graph-embedded' $Suffix
$OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
$artifactRoot = [IO.Path]::GetFullPath((Join-Path $script:DatabaseRoot 'artifacts')) + [IO.Path]::DirectorySeparatorChar
if (-not $OutputDirectory.StartsWith($artifactRoot, [StringComparison]::OrdinalIgnoreCase)) { throw 'Diagnostics must be inside database/artifacts.' }
if ((Test-Path -LiteralPath $OutputDirectory) -and @(Get-ChildItem -LiteralPath $OutputDirectory -Force).Count -gt 0) {
    throw 'Use a new empty output directory to preserve previous diagnostic evidence.'
}
foreach ($volume in @($DatasetVolume,$PristineVolume,$ToolsVolume)) {
    if ($volume -notmatch '^dbbench-[a-z0-9-]+$') { throw 'Diagnostics require named dbbench volumes.' }
}
New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
foreach ($volume in @($PristineVolume,$ToolsVolume)) { Assert-BenchOwner $volume -Volume }
$image = "localhost/dbbench-driver:$Tag"
$driver = "dbbench-graph-memory$Suffix"
$collector = "dbbench-graph-counters$Suffix"
$dump = "dbbench-graph-gcdump$Suffix"
$copy = "dbbench-graph-memory-copy$Suffix"
$working = "dbbench-graph-diagnostic-data$Suffix"
$ipc = "dbbench-graph-diagnostic-ipc$Suffix"
foreach ($name in @($driver,$collector,$dump,$copy)) {
    & podman container exists $name
    if ($LASTEXITCODE -eq 0) { throw "Refusing existing diagnostic container: $name" }
}
foreach ($volume in @($working,$ipc)) {
    & podman volume exists $volume
    if ($LASTEXITCODE -eq 0) { throw "Refusing existing diagnostic volume: $volume" }
}
$createdContainers = [Collections.Generic.List[string]]::new()
$createdVolumes = [Collections.Generic.List[string]]::new()
$exitCode = 1
$timedOut = $false
try {
    foreach ($volume in @($working,$ipc)) {
        Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",$volume) | Out-Null
        $createdVolumes.Add($volume)
    }
    $createdContainers.Add($copy)
    Invoke-Podman @('run','--rm','--name',$copy,'--label',"dbbench.owner=$script:Owner",'--network','none',
        '--cpus','1','--memory','512m','--memory-swap','512m','--volume',"${PristineVolume}:/from:ro",'--volume',"${working}:/to",
        '--entrypoint','/bin/sh',$image,'-c','cp -a /from/. /to/')
    $null = $createdContainers.Remove($copy)
    $scaleText = $Scale.ToString('0.################', [Globalization.CultureInfo]::InvariantCulture)
    $warmup = $WarmupSeconds.ToString([Globalization.CultureInfo]::InvariantCulture) + 's'
    $duration = $DurationSeconds.ToString([Globalization.CultureInfo]::InvariantCulture) + 's'
    $command = @('run','--model','graph','--target','cohesion-graph-embedded','--data',"/data/graph/sf$scaleText",
        '--suite','throughput','--clients',($Clients -join ','),'--warmup',$warmup,'--duration',$duration,'--repeat','1',
        '--buffer-pool-mb',[string]$BufferPoolMb,'--out','/results')
    if ($Operations) { $command += @('--ops',($Operations -join ',')) }
    @{image=$image;scale=$Scale;memory=$Memory;bufferPoolMb=$BufferPoolMb;command=$command;dumpAtSeconds=$DumpAtSeconds;
        note='Diagnostic only: counters and optional forced-GC dumps; not comparable performance.'} |
        ConvertTo-Json -Depth 10 | Set-Content -LiteralPath (Join-Path $OutputDirectory 'diagnostic-command.json')
    $createdContainers.Add($driver)
    Invoke-Podman (@('run','-d','--name',$driver,'--label',"dbbench.owner=$script:Owner",'--network','dbbench',
        '--cpus','3','--memory',$Memory,'--memory-swap',$Memory,'--env','DOTNET_DefaultDiagnosticPortSuspend=1','--env','DBBENCH_DIAGNOSTIC_STACKS=1',
        '--volume',"${working}:/engine-data",'--volume',"${DatasetVolume}:/data:ro",'--volume',"${ipc}:/tmp",
        '--volume',"${OutputDirectory}:/results",$image) + $command) | Out-Null
    # Shared PID and /tmp namespaces expose the target's default diagnostics socket. Counters resume its runtime.
    $createdContainers.Add($collector)
    Invoke-Podman @('run','-d','--name',$collector,'--label',"dbbench.owner=$script:Owner",'--network','none',
        '--pid',"container:$driver",'--cpus','1','--memory','1g','--memory-swap','1g','--volume',"${ipc}:/tmp",
        '--volume',"${ToolsVolume}:/tools:ro",'--volume',"${OutputDirectory}:/results",$SdkImage,
        '/tools/dotnet-counters','collect','--process-id','1','--counters','System.Runtime','--format','csv',
        '--output','/results/runtime','--duration',([TimeSpan]::FromSeconds($MaximumSeconds).ToString('c'))) | Out-Null
    $watch = [Diagnostics.Stopwatch]::StartNew()
    $remainingDumps = [Collections.Generic.Queue[int]]::new()
    foreach ($second in ($DumpAtSeconds | Sort-Object)) { $remainingDumps.Enqueue($second) }
    while ((Invoke-Podman @('inspect','--format','{{.State.Running}}',$driver)) -eq 'true') {
        if ($watch.Elapsed.TotalSeconds -ge $MaximumSeconds) {
            $timedOut = $true
            Invoke-Podman @('stop','--time','10',$driver) | Out-Null
            break
        }
        if ((Invoke-Podman @('inspect','--format','{{.State.Running}}',$collector)) -eq 'false') {
            $collectorExit = Invoke-Podman @('inspect','--format','{{.State.ExitCode}}',$collector)
            if ($collectorExit -ne '0') { throw "Runtime counter collector failed ($collectorExit); inspect counters.log." }
        }
        if ($remainingDumps.Count -gt 0 -and $watch.Elapsed.TotalSeconds -ge $remainingDumps.Peek()) {
            $second = $remainingDumps.Dequeue()
            Write-Host "$([DateTimeOffset]::UtcNow.ToString('O')) GCDUMP at requested second $second"
            $createdContainers.Add($dump)
            & podman run --name $dump --label "dbbench.owner=$script:Owner" --network none --pid "container:$driver" `
                --cpus 3 --memory 3g --memory-swap 3g --volume "${ipc}:/tmp" --volume "${ToolsVolume}:/tools:ro" `
                --volume "${OutputDirectory}:/results" $SdkImage /tools/dotnet-gcdump collect --process-id 1 `
                --timeout 60 --output "/results/heap-$second.gcdump" *> (Join-Path $OutputDirectory "gcdump-$second.log")
            Write-Host "GCDUMP exit=$LASTEXITCODE"
            & podman container exists $dump
            if ($LASTEXITCODE -eq 0) {
                Assert-BenchOwner $dump
                Invoke-Podman @('inspect',$dump) | Set-Content -LiteralPath (Join-Path $OutputDirectory "gcdump-$second-inspect.json")
                Invoke-Podman @('rm','-f',$dump) | Out-Null
            }
            $null = $createdContainers.Remove($dump)
        }
        & podman stats --no-stream --format json $driver 2>$null | Add-Content -LiteralPath (Join-Path $OutputDirectory 'container-stats.log')
        Start-Sleep -Seconds 1
    }
    $inspect = Invoke-Podman @('inspect',$driver) | Out-String
    [IO.File]::WriteAllText((Join-Path $OutputDirectory 'container-inspect.json'),$inspect)
    $state = @($inspect | ConvertFrom-Json -AsHashtable)[0].State
    $exitCode = [int]$state.ExitCode
    & podman logs $driver *> (Join-Path $OutputDirectory 'console.log')
    Start-Sleep -Seconds 2
    @{exitCode=$exitCode;oomKilled=$state.OOMKilled;timedOut=$timedOut;seconds=$watch.Elapsed.TotalSeconds} |
        ConvertTo-Json | Set-Content -LiteralPath (Join-Path $OutputDirectory 'diagnostic-status.json')
    Write-Host "DIAGNOSTIC END exit=$exitCode oomKilled=$($state.OOMKilled) timedOut=$timedOut seconds=$($watch.Elapsed.TotalSeconds)"
} finally {
    for ($index = $createdContainers.Count - 1; $index -ge 0; $index--) {
        $name = $createdContainers[$index]
        & podman container exists $name
        if ($LASTEXITCODE -eq 0) {
            Assert-BenchOwner $name
            if ($name -eq $driver) {
                & podman logs $name *> (Join-Path $OutputDirectory 'console.log')
                Invoke-Podman @('inspect',$name) | Set-Content -LiteralPath (Join-Path $OutputDirectory 'container-inspect.json')
            }
            if ($name -eq $collector) {
                & podman stop --time 10 $name *> $null
                & podman logs $name *> (Join-Path $OutputDirectory 'counters.log')
            }
            Invoke-Podman @('rm','-f',$name) | Out-Null
        }
    }
    foreach ($volume in $createdVolumes) { Assert-BenchOwner $volume -Volume; Invoke-Podman @('volume','rm',$volume) | Out-Null }
}
# Preserve nonzero workload exits as evidence; the diagnostic controller itself completed successfully.
exit 0
