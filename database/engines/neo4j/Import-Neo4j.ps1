[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidatePattern('^dbbench-neo4j(-[a-z0-9-]+)?$')][string]$Container,
    [Parameter(Mandatory)][string]$InputDirectory
)
# Invoked by Neo4jOfflineImport after it has checked that the database is empty.
. (Join-Path $PSScriptRoot '../../scripts/BenchEngine.Common.ps1')
Assert-BenchOwner $Container
$info = @(Invoke-Podman @('inspect', $Container) | ConvertFrom-Json -AsHashtable)[0]
$mount = @($info.Mounts | Where-Object Destination -eq '/data')
if ($mount.Count -ne 1 -or $mount[0].Type -ne 'volume') { throw 'Neo4j data must be a named volume.' }
Assert-BenchOwner $mount[0].Name -Volume
$helper = $Container.Replace('dbbench-neo4j', 'dbbench-neo4j-import')
$inputVolume = $Container.Replace('dbbench-neo4j', 'dbbench-neo4j-import-input')
$importArguments = [string[]](Get-Content -LiteralPath (Join-Path $InputDirectory 'arguments.json') -Raw | ConvertFrom-Json)
& podman container exists $helper
if ($LASTEXITCODE -eq 0) { throw "Helper already exists: $helper" }
& podman volume exists $inputVolume
if ($LASTEXITCODE -eq 0) { throw "Input volume already exists: $inputVolume" }
$created = $false
$stopped = $false
try {
    Invoke-Podman @('stop', '--time', '40', $Container) | Out-Null
    $stopped = $true
    Invoke-Podman @('volume', 'create', '--label', "dbbench.owner=$script:Owner", $inputVolume) | Out-Null
    $created = $true
    $memory = [string]$info.HostConfig.Memory
    $cpus = ([double]$info.HostConfig.NanoCpus / 1000000000).ToString([Globalization.CultureInfo]::InvariantCulture)
    Invoke-Podman @('run', '-d', '--name', $helper, '--label', "dbbench.owner=$script:Owner", '--network', 'none',
        '--cpus', $cpus, '--memory', $memory, '--memory-swap', $memory, '--image-volume', 'ignore',
        '--user', 'root', '-e', 'HEAP_SIZE=512m', '-v', "$($mount[0].Name):/data", '-v', "${inputVolume}:/import",
        '--entrypoint', 'sleep', $info.ImageName, 'infinity') | Out-Null
    Invoke-Podman @('cp', ((Resolve-Path -LiteralPath $InputDirectory).Path + '/.'), "${helper}:/import/") | Out-Null
    $watch = [Diagnostics.Stopwatch]::StartNew()
    Invoke-Podman (@('exec', '--user', 'neo4j', $helper, 'neo4j-admin') + $importArguments)
    $watch.Stop()
    @{seconds=$watch.Elapsed.TotalSeconds; path='neo4j-admin database import full'} | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $InputDirectory 'receipt.json')
} finally {
    & podman container exists $helper
    if ($LASTEXITCODE -eq 0) { Invoke-Podman @('rm', '-f', $helper) | Out-Null }
    if ($created) { Invoke-Podman @('volume', 'rm', $inputVolume) | Out-Null }
    if ($stopped) { Invoke-Podman @('start', $Container) | Out-Null }
}
