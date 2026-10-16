param(
    [Parameter(Mandatory)][string]$Container,
    [Parameter(Mandatory)][string]$InputVolume,
    [Parameter(Mandatory)][string]$ControlDirectory,
    [Parameter(Mandatory)][string]$Suffix,
    [Parameter(Mandatory)][string]$CommonScript
)
$ErrorActionPreference = 'Stop'
$request = Join-Path $ControlDirectory 'import.request.json'
$receipt = Join-Path $ControlDirectory 'import.receipt.json'
$helper = "dbbench-neo4j-import$Suffix"
$created = $false
$stopped = $false
try {
    # Start-ThreadJob -FilePath does not supply PSScriptRoot; use the caller's resolved path.
    . $CommonScript
    while (-not (Test-Path -LiteralPath $request)) { Start-Sleep -Seconds 1 }
    Assert-BenchOwner $Container
    Assert-BenchOwner $InputVolume -Volume
    $info = @(Invoke-Podman @('inspect', $Container) | ConvertFrom-Json -AsHashtable)[0]
    $mount = @($info.Mounts | Where-Object Destination -eq '/data')
    if ($mount.Count -ne 1 -or $mount[0].Type -ne 'volume') { throw 'Neo4j requires a named data volume.' }
    Assert-BenchOwner $mount[0].Name -Volume
    & podman container exists $helper
    if ($LASTEXITCODE -eq 0) { throw "Refusing existing import helper $helper" }
    Invoke-Podman @('stop','--time','40',$Container) | Out-Null
    $stopped = $true
    $memory = [long]$info.HostConfig.Memory
    $cpus = ([double]$info.HostConfig.NanoCpus / 1000000000).ToString([Globalization.CultureInfo]::InvariantCulture)
    Invoke-Podman @('run','-d','--name',$helper,'--label',"dbbench.owner=$script:Owner",'--network','none',
        '--cpus',$cpus,'--memory',[string]$memory,'--memory-swap',[string]$memory,'--image-volume','ignore',
        '--user','root','--env',"HEAP_SIZE=$([long]($memory / 4))",'--volume',"$($mount[0].Name):/data",
        '--volume',"${InputVolume}:/import:ro",'--entrypoint','sleep',$info.ImageName,'infinity') | Out-Null
    $created = $true
    $arguments = [string[]]((Invoke-Podman @('exec',$helper,'cat','/import/arguments.json') | Out-String) | ConvertFrom-Json)
    $arguments += "--max-off-heap-memory=$([long]($memory / 2))"
    $watch = [Diagnostics.Stopwatch]::StartNew()
    Invoke-Podman (@('exec','--user','neo4j',$helper,'neo4j-admin') + $arguments) 2>&1 |
        Tee-Object -FilePath (Join-Path $ControlDirectory 'import.log') | Write-Output
    $watch.Stop()
    Invoke-Podman @('rm','-f',$helper) | Out-Null
    $created = $false
    Invoke-Podman @('start',$Container) | Out-Null
    $stopped = $false
    $result = @{ seconds = $watch.Elapsed.TotalSeconds }
} catch { $result = @{ error = $_.Exception.Message } }
finally {
    if ($created) { Invoke-Podman @('rm','-f',$helper) | Out-Null }
    if ($stopped) { Invoke-Podman @('start',$Container) | Out-Null }
}
# Atomic publication prevents the driver from reading a partial receipt.
$temporary = "$receipt.tmp"
[IO.File]::WriteAllText($temporary, ($result | ConvertTo-Json))
Move-Item -LiteralPath $temporary -Destination $receipt -Force
