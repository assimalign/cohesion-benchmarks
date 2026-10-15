[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Target,
    [ValidateRange(0.1, 128)][double]$Cpus = 3,
    [ValidatePattern('^[1-9][0-9]*[mg]$')][string]$Memory = '6g',
    [string]$Suffix = '',
    [switch]$Recreate,
    [ValidateRange(5, 1800)][int]$TimeoutSeconds = 180,
    [string]$Tag = 'dev'
)
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
$engine = Get-BenchTarget $Target $Suffix
if ($engine.kind -eq 'embedded') { Write-Output "embedded:$($engine.volume)$Suffix"; return }
$name = $engine.containerName + $Suffix
$volume = $engine.volume + $Suffix
& podman network exists dbbench
if ($LASTEXITCODE -ne 0) { Invoke-Podman @('network', 'create', '--label', 'dbbench.shared=true', 'dbbench') | Out-Null }
& podman volume exists $volume
if ($LASTEXITCODE -ne 0) { Invoke-Podman @('volume', 'create', '--label', "dbbench.owner=$script:Owner", $volume) | Out-Null }
else { Assert-BenchOwner $volume -Volume }
& podman container exists $name
$exists = $LASTEXITCODE -eq 0
if ($exists) {
    Assert-BenchOwner $name
    if (-not $Recreate) { throw "$name already exists; use -Recreate to replace this worktree's container." }
    Invoke-Podman @('stop', '--time', '40', $name) | Out-Null
    Invoke-Podman @('rm', '--volumes', $name) | Out-Null
}
$envValues = @{}
foreach ($key in $engine.environment.Keys) { $envValues[$key] = Expand-BenchValue $engine.environment[$key] $engine }
$engineArgs = [Collections.Generic.List[string]]::new()
foreach ($argument in $engine.arguments) { $engineArgs.Add([string]$argument) }
$memoryMb = [int]$Memory.Substring(0, $Memory.Length - 1) * $(if ($Memory.EndsWith('g')) { 1024 } else { 1 })
$profile = 'official-budget'
if ($memoryMb -lt 6144) {
    $profile = 'development'
    if ($Target -eq 'postgresql') {
        $engineArgs.AddRange([string[]]@('-c', "shared_buffers=$([int]($memoryMb / 4))MB", '-c', "effective_cache_size=$([int]($memoryMb / 2))MB",
            '-c', 'work_mem=16MB', '-c', "maintenance_work_mem=$([Math]::Max(64, [int]($memoryMb / 8)))MB"))
    }
    if ($Target -eq 'sqlserver') { $envValues.MSSQL_MEMORY_LIMIT_MB = [string][int]($memoryMb * 0.75) }
    if ($Target -eq 'mongodb') { $engineArgs.Clear(); $engineArgs.AddRange([string[]]@('--wiredTigerCacheSizeGB', [Math]::Max(0.25, $memoryMb / 1024 * 0.4).ToString('0.###', [Globalization.CultureInfo]::InvariantCulture))) }
    if ($Target -eq 'neo4j') {
        $envValues.NEO4J_server_memory_heap_initial__size = "$([int]($memoryMb * 0.1875))m"
        $envValues.NEO4J_server_memory_heap_max__size = $envValues.NEO4J_server_memory_heap_initial__size
        $envValues.NEO4J_server_memory_pagecache_size = "$([int]($memoryMb * 0.25))m"
    }
    if ($Target -like 'cohesion-*-tcp') {
        $position = $engineArgs.IndexOf('--buffer-pool-mb')
        $engineArgs[$position + 1] = [string][int]($memoryMb / 2)
    }
}
$runArgs = [Collections.Generic.List[string]]::new()
$runArgs.AddRange([string[]]@('run', '--detach', '--image-volume', 'ignore', '--name', $name, '--network', 'dbbench', '--network-alias', ($engine.networkAlias + $Suffix),
    '--cpus', $Cpus.ToString([Globalization.CultureInfo]::InvariantCulture), '--memory', $Memory, '--memory-swap', $Memory,
    '--label', "dbbench.owner=$script:Owner", '--label', "dbbench.profile=$profile", '--volume', "${volume}:$($engine.mountPath):U"))
foreach ($port in $engine.ports) { $runArgs.AddRange([string[]]@('--publish', "127.0.0.1:$($port.published):$($port.internal)")) }
foreach ($key in $envValues.Keys) { $runArgs.AddRange([string[]]@('--env', "$key=$($envValues[$key])")) }
$image = $engine.image
if ($Target -like 'cohesion-*-tcp') { $image = "localhost/dbbench-cohesion-server:$Tag" }
$runArgs.Add($image)
$runArgs.AddRange($engineArgs)
$created = $false
try {
    Invoke-Podman $runArgs.ToArray() | Out-Null
    $created = $true
    $deadline = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds)
    do {
        $state = Invoke-Podman @('inspect', '--format', '{{.State.Status}}', $name)
        if ($state -ne 'running') { throw "$name is $state." }
        $ready = $false
        switch ($engine.readiness.kind) {
            'exec' {
                $probe = @('exec', $name, 'timeout', '10s') + @($engine.readiness.command | ForEach-Object { Expand-BenchValue $_ $engine })
                & podman @probe *> $null
                $ready = $LASTEXITCODE -eq 0
            }
            'http' {
                try {
                    $uri = "$($engine.readiness.scheme)://localhost:$($engine.ports[0].published)$($engine.readiness.path)"
                    $response = Invoke-WebRequest -Uri $uri -TimeoutSec 3 -SkipCertificateCheck
                    $ready = $response.StatusCode -eq 200
                } catch { }
            }
            { $_ -in 'log', 'cohesion' } {
                $logs = (& podman logs --tail 150 $name 2>&1 | Out-String)
                $ready = $logs -match $engine.readiness.pattern
                if ($ready) {
                    $tcp = [Net.Sockets.TcpClient]::new()
                    try { $tcp.ConnectAsync('localhost', [int]$engine.ports[0].published).WaitAsync([TimeSpan]::FromSeconds(3)).GetAwaiter().GetResult() | Out-Null }
                    catch { $ready = $false }
                    finally { $tcp.Dispose() }
                }
                if ($ready -and $engine.readiness.kind -eq 'cohesion') {
                    $probeName = "dbbench-probe-$Target$Suffix"
                    & podman run --rm --name $probeName --network dbbench --cpus 1 --memory 256m --memory-swap 256m "localhost/dbbench-driver:$Tag" ping --target $Target --endpoint "tcp://${name}:5740" *> $null
                    $ready = $LASTEXITCODE -eq 0
                }
            }
        }
        if ($ready) {
            $endpoint = $engine.endpointTemplate.Replace('{host}', 'localhost').Replace('{port}', [string]$engine.ports[0].published)
            Write-Host "READY $name profile=$profile cpus=$Cpus memory=$Memory"
            Write-Output $endpoint
            return
        }
        Start-Sleep -Seconds 1
    } while ([DateTime]::UtcNow -lt $deadline)
    throw "$name readiness timed out after $TimeoutSeconds seconds."
} catch {
    if ($created) {
        & podman logs --tail 60 $name 2>&1 | Write-Host
        & podman stop --time 40 $name | Out-Null
        & podman rm --volumes $name | Out-Null
    }
    throw
}
