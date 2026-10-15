[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('neo4j','embedded','embedded-throughput','tcp','report','cleanup')][string]$Phase,
    [ValidatePattern('^-[a-z0-9-]+$')][string]$Suffix = '-r4',
    [string]$Tag = 'r4',
    [switch]$ReuseNeo4jLoad
)
. (Join-Path $PSScriptRoot '../../scripts/BenchEngine.Common.ps1')
$cli = Join-Path $script:DatabaseRoot 'artifacts/bin/Assimalign.Cohesion.Benchmarks.Database.Cli/release/dbbench.dll'
$key = $Suffix.TrimStart('-')
$data = Join-Path $script:DatabaseRoot "artifacts/$key-data/graph/sf0.01"
$results = Join-Path $script:DatabaseRoot "artifacts/$key-results"
$datasetVolume = "dbbench-cohesion-graph-datasets$Suffix"
$graphVolume = "dbbench-cohesion-graph-data$Suffix"
New-Item -ItemType Directory -Force -Path $results | Out-Null
$script:MeasuredErrors = $false

function Assert-GraphContainers([string]$OwnContainer = '') {
    $running = @(podman ps --format '{{.Names}}')
    $owned = @($running | Where-Object { $_ -like "dbbench-*$Suffix" -and $_ -ne $OwnContainer })
    if ($owned.Count -ne 0) { throw "Another R4 container is active: $($owned -join ', ')" }
    $others = @($running | Where-Object { $_ -like 'dbbench-*' -and $_ -ne $OwnContainer })
    Write-Host "SHARED-HOST smoke only; other containers: $($others -join ', ')"
    @{capturedUtc=(Get-Date -AsUTC -Format o); phase=$Phase; otherContainers=$others} |
        ConvertTo-Json -Compress | Add-Content -LiteralPath (Join-Path $results 'host-activity.jsonl')
}
function New-OwnedVolume([string]$Name) {
    & podman volume exists $Name
    if ($LASTEXITCODE -eq 0) { Assert-BenchOwner $Name -Volume }
    else { Invoke-Podman @('volume','create','--label',"dbbench.owner=$script:Owner",$Name) | Out-Null }
}
function Initialize-DatasetVolume {
    New-OwnedVolume $datasetVolume
    $name = "dbbench-cohesion-graph-stage$Suffix"
    & podman run --rm --name $name --cpus 1 --memory 256m --memory-swap 256m --image-volume ignore `
        -v "${datasetVolume}:/data" --entrypoint /usr/bin/test "localhost/dbbench-driver:$Tag" -f /data/graph/sf0.01/manifest.json
    if ($LASTEXITCODE -ne 0) {
        Invoke-Podman @('run','--rm','--name',$name,'--label',"dbbench.owner=$script:Owner",'--cpus','3',
            '--memory','2g','--memory-swap','2g','--image-volume','ignore','-v',"${datasetVolume}:/data",
            "localhost/dbbench-driver:$Tag",'generate','--model','graph','--scale','0.01','--out','/data')
    }
    $volumeManifest = Invoke-Podman @('run','--rm','--name',$name,'--cpus','1','--memory','256m','--memory-swap','256m',
        '--image-volume','ignore','-v',"${datasetVolume}:/data:ro",'--entrypoint','/usr/bin/cat',
        "localhost/dbbench-driver:$Tag",'/data/graph/sf0.01/manifest.json')
    # JSON indentation uses the platform newline; compare every manifest field,
    # including all data-part SHA256 hashes, rather than the JSON file's bytes.
    $volumeJson = ($volumeManifest -join "`n") | ConvertFrom-Json | ConvertTo-Json -Depth 100 -Compress
    $hostJson = Get-Content -Raw -LiteralPath (Join-Path $data 'manifest.json') | ConvertFrom-Json | ConvertTo-Json -Depth 100 -Compress
    if ($volumeJson -cne $hostJson) {
        throw 'Host and named-volume dataset manifests differ.'
    }
    # Verification records the manifest file's hash. Preserve the exact host file
    # after checking that its contents describe the identical generated parts.
    Invoke-Podman @('run','-d','--name',$name,'--label',"dbbench.owner=$script:Owner",'--cpus','1',
        '--memory','256m','--memory-swap','256m','--image-volume','ignore','-v',"${datasetVolume}:/data",
        '--entrypoint','/usr/bin/sleep',"localhost/dbbench-driver:$Tag",'infinity') | Out-Null
    try {
        Invoke-Podman @('cp',(Join-Path $data 'manifest.json'),"${name}:/data/graph/sf0.01/manifest.json") | Out-Null
        $hash = Invoke-Podman @('exec',$name,'sha256sum','/data/graph/sf0.01/manifest.json')
        if (($hash -split '\s+')[0] -ne (Get-FileHash -LiteralPath (Join-Path $data 'manifest.json') -Algorithm SHA256).Hash.ToLowerInvariant()) {
            throw 'Manifest byte copy failed.'
        }
    } finally { Invoke-Podman @('rm','-f',$name) | Out-Null }
}
function Invoke-Driver([string]$Target, [string]$Command, [string[]]$Extra = @(), [switch]$RecordFatal) {
    $embedded = $Target -eq 'cohesion-graph-embedded'
    $name = if ($embedded) { "dbbench-cohesion-graph$Suffix" } elseif ($Target -eq 'neo4j') { "dbbench-neo4j-driver$Suffix" } else { "dbbench-cohesion-graph-driver$Suffix" }
    $driverArgs = @('run','--rm','--name',$name,'--label',"dbbench.owner=$script:Owner",'--network','dbbench',
        '--cpus','3','--memory','2g','--memory-swap','2g','--image-volume','ignore',
        '--mount',"type=bind,source=$results,target=/results",'-v',"${datasetVolume}:/data:ro")
    if ($Target -ne 'neo4j') { $driverArgs += @('-v', $(if ($embedded) { "${graphVolume}:/engine-data" } else { "${graphVolume}:/engine-data:ro" })) }
    $driverArgs += @("localhost/dbbench-driver:$Tag",$Command,'--model','graph','--target',$Target,'--suffix',$Suffix,
        '--data','/data/graph/sf0.01','--buffer-pool-mb','1024')
    if (-not $embedded -and $Command -ne 'verify') { $driverArgs += @('--environment',"/results/$Target.environment.json") }
    $driverArgs += $Extra
    $logPhase = if ($Command -eq 'run') { $Extra[([Array]::IndexOf($Extra,'--suite') + 1)] } else { $Command }
    if ($Extra -contains '--ops') {
        $logPhase += '-' + $Extra[([Array]::IndexOf($Extra,'--ops') + 1)] + '-c' + $Extra[([Array]::IndexOf($Extra,'--clients') + 1)]
    }
    $log = Join-Path $script:DatabaseRoot "artifacts/$key-smoke-$Target-$logPhase.log"
    & podman @driverArgs 2>&1 | Tee-Object -FilePath $log
    $code = $LASTEXITCODE
    Add-Content -LiteralPath $log -Value "SMOKE $Target $Command exit=$code"
    Write-Host "SMOKE $Target $Command exit=$code"
    if ($code -eq 2) { $script:MeasuredErrors = $true }
    elseif ($RecordFatal -and (($code -eq 1 -and (Get-Content -Raw -LiteralPath $log) -match 'OutOfMemoryException') -or $code -eq 137)) {
        $script:MeasuredErrors = $true
        $verification = Get-Content -Raw -LiteralPath (Join-Path $results "$Target.verify.json") | ConvertFrom-Json
        @{model='graph'; target=$Target; operation=$Extra[([Array]::IndexOf($Extra,'--ops') + 1)];
          clients=$Extra[([Array]::IndexOf($Extra,'--clients') + 1)]; exitCode=$code;
          datasetManifestSha256=$verification.datasetManifestSha256; cpus=3; memoryBytes=2147483648;
          evidence=(Get-Content -Raw -LiteralPath $log).Trim(); command=$driverArgs} |
            ConvertTo-Json -Depth 10 | Set-Content -LiteralPath (Join-Path $results "$Target-$logPhase.aborted.json")
    }
    elseif ($code -ne 0) { throw "Smoke $Target $Command failed with exit $code; see $log" }
}
function Invoke-VerifyAndRuns([string]$Target) {
    Invoke-Driver $Target verify @('--out',"/results/$Target.verify.json")
    Invoke-Driver $Target run @('--suite','latency','--clients','1','--duration','10s','--warmup','1s','--iterations','2147483647',
        '--repeat','1','--out','/results','--verification',"/results/$Target.verify.json")
    if ($Target -eq 'cohesion-graph-embedded') { Invoke-IsolatedThroughput $Target }
    else {
        Invoke-Driver $Target run @('--suite','throughput','--clients','1,8','--duration','10s','--warmup','1s',
            '--repeat','1','--out','/results','--verification',"/results/$Target.verify.json")
    }
}
function Invoke-IsolatedThroughput([string]$Target) {
    $verification = Get-Content -Raw -LiteralPath (Join-Path $results "$Target.verify.json") | ConvertFrom-Json
    foreach ($operation in $verification.operations) {
        if ($operation.support.level -eq 'Unsupported') { continue }
        foreach ($clients in @('1','8')) {
            Invoke-Driver $Target run @('--suite','throughput','--clients',$clients,'--duration','10s','--warmup','1s',
                '--repeat','1','--ops',$operation.operation,'--out','/results',
                '--verification',"/results/$Target.verify.json") -RecordFatal
        }
    }
}

switch ($Phase) {
    'neo4j' {
        Assert-GraphContainers "dbbench-neo4j$Suffix"
        Initialize-DatasetVolume
        & podman container exists "dbbench-neo4j$Suffix"
        if ($LASTEXITCODE -eq 0) { Assert-BenchOwner "dbbench-neo4j$Suffix"; Invoke-Podman @('start',"dbbench-neo4j$Suffix") | Out-Null }
        else { & (Join-Path $script:DatabaseRoot 'scripts/Start-BenchEngine.ps1') -Target neo4j -Memory 2g -Suffix $Suffix -Tag $Tag }
        try {
            $ready = $false
            for ($i = 0; $i -lt 90; $i++) {
                & dotnet $cli ping --target neo4j --suffix $Suffix *> $null
                if ($LASTEXITCODE -eq 0) { $ready = $true; break }
                Start-Sleep -Seconds 1
            }
            if (-not $ready) { throw 'Neo4j readiness failed.' }
            & dotnet $cli env --container "dbbench-neo4j$Suffix" --out (Join-Path $results 'neo4j.environment.json') *> $null
            if ($LASTEXITCODE -ne 0) { throw 'Neo4j host environment capture failed.' }
            if (-not $ReuseNeo4jLoad) {
                & dotnet $cli load --model graph --target neo4j --suffix $Suffix --data $data --out $results --batch 1000 2>&1 |
                    Tee-Object -FilePath (Join-Path $script:DatabaseRoot "artifacts/$key-smoke-neo4j-load.log")
                if ($LASTEXITCODE -ne 0) { throw 'Neo4j full load failed.' }
            }
            Invoke-VerifyAndRuns neo4j
        } finally {
            & (Join-Path $script:DatabaseRoot 'scripts/Stop-BenchEngine.ps1') -Target neo4j -Suffix $Suffix -RemoveData
        }
    }
    'embedded' {
        Assert-GraphContainers
        Initialize-DatasetVolume
        New-OwnedVolume $graphVolume
        Invoke-Driver cohesion-graph-embedded load @('--out','/results','--batch','1000')
        Invoke-VerifyAndRuns cohesion-graph-embedded
    }
    'embedded-throughput' {
        Assert-GraphContainers
        Assert-BenchOwner $graphVolume -Volume
        Assert-BenchOwner $datasetVolume -Volume
        Invoke-IsolatedThroughput cohesion-graph-embedded
    }
    'tcp' {
        Assert-GraphContainers
        & (Join-Path $script:DatabaseRoot 'scripts/Start-BenchEngine.ps1') -Target cohesion-graph-tcp -Memory 2g -Suffix $Suffix -Tag $Tag
        try {
            Invoke-Podman @('exec',"dbbench-cohesion-graph$Suffix",'cat','/proc/1/status') |
                Set-Content -LiteralPath (Join-Path $results 'cohesion-graph-tcp-open-memory.txt')
            & dotnet $cli env --container "dbbench-cohesion-graph$Suffix" --out (Join-Path $results 'cohesion-graph-tcp.environment.json') *> $null
            if ($LASTEXITCODE -ne 0) { throw 'Cohesion host environment capture failed.' }
            Invoke-Driver cohesion-graph-tcp load @('--out','/results','--batch','1000')
            # The server caches opened databases. Restart after the isolated ingest
            # sample so its id map/cache cannot consume the baseline's engine budget.
            & (Join-Path $script:DatabaseRoot 'scripts/Stop-BenchEngine.ps1') -Target cohesion-graph-tcp -Suffix $Suffix
            & (Join-Path $script:DatabaseRoot 'scripts/Start-BenchEngine.ps1') -Target cohesion-graph-tcp -Memory 2g -Suffix $Suffix -Tag $Tag
            Invoke-Podman @('exec',"dbbench-cohesion-graph$Suffix",'cat','/proc/1/status') |
                Set-Content -LiteralPath (Join-Path $results 'cohesion-graph-tcp-reopened-memory.txt')
            & dotnet $cli env --container "dbbench-cohesion-graph$Suffix" --out (Join-Path $results 'cohesion-graph-tcp.environment.json') *> $null
            if ($LASTEXITCODE -ne 0) { throw 'Cohesion reopened environment capture failed.' }
            Invoke-VerifyAndRuns cohesion-graph-tcp
        } finally {
            & (Join-Path $script:DatabaseRoot 'scripts/Stop-BenchEngine.ps1') -Target cohesion-graph-tcp -Suffix $Suffix -RemoveData
        }
    }
    'report' {
        & dotnet $cli verify --compare $results --out (Join-Path $results 'comparison.json')
        if ($LASTEXITCODE -eq 2) { $script:MeasuredErrors = $true } elseif ($LASTEXITCODE -ne 0) { throw 'Comparison failed.' }
        & dotnet $cli report --in $results --out (Join-Path $script:DatabaseRoot "artifacts/$key-report")
        if ($LASTEXITCODE -ne 0) { throw 'Report rendering failed.' }
        # The foundation writes a suite result only on normal completion. Preserve
        # process-level failures separately; never invent operation samples for them.
        $aborts = @(Get-ChildItem -LiteralPath $results -Filter '*.aborted.json')
        if ($aborts.Count) {
            $report = Join-Path $script:DatabaseRoot "artifacts/$key-report"
            $findingsPath = Join-Path $report 'findings.json'
            $findings = @(Get-Content -Raw -LiteralPath $findingsPath | ConvertFrom-Json)
            Add-Content -LiteralPath (Join-Path $report 'graph.md') -Value "`n## Aborted smoke processes`n`nThese attempts produced no histogram and are not successful measurements.`n"
            foreach ($file in $aborts) {
                $abort = Get-Content -Raw -LiteralPath $file.FullName | ConvertFrom-Json
                $evidence = "clients=$($abort.clients), exit=$($abort.exitCode), 3 CPUs/2 GiB: $($abort.evidence)"
                Add-Content -LiteralPath (Join-Path $report 'graph.md') -Value "* $($abort.target), $($abort.operation): $evidence"
                $findings += @{model='graph'; cohort="fatal-smoke:$($abort.datasetManifestSha256)"; target=$abort.target;
                    kind='fatal'; operation=$abort.operation; evidence=$evidence; value=$null; best=$null; ratio=$null; bestTarget=$null}
            }
            $findings | ConvertTo-Json -Depth 100 | Set-Content -LiteralPath $findingsPath
        }
    }
    'cleanup' {
        foreach ($target in @('neo4j','cohesion-graph-tcp')) {
            & (Join-Path $script:DatabaseRoot 'scripts/Stop-BenchEngine.ps1') -Target $target -Suffix $Suffix -RemoveData
        }
        & podman volume exists $datasetVolume
        if ($LASTEXITCODE -eq 0) { Assert-BenchOwner $datasetVolume -Volume; Invoke-Podman @('volume','rm',$datasetVolume) | Out-Null }
    }
}
if ($script:MeasuredErrors) { exit 2 }
