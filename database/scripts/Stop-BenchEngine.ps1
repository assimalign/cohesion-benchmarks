[CmdletBinding()]
param([Parameter(Mandatory)][string]$Target, [string]$Suffix = '', [switch]$RemoveData)
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
$engine = Get-BenchTarget $Target $Suffix
$name = $engine.containerName + $Suffix
if ($engine.kind -ne 'embedded') {
    & podman container exists $name
    if ($LASTEXITCODE -eq 0) {
        Assert-BenchOwner $name
        Invoke-Podman @('stop', '--time', '40', $name) | Out-Null
        Invoke-Podman @('rm', $name) | Out-Null
        Write-Host "REMOVED $name"
    }
}
if ($RemoveData) {
    $volume = $engine.volume + $Suffix
    & podman volume exists $volume
    if ($LASTEXITCODE -eq 0) {
        Assert-BenchOwner $volume -Volume
        Invoke-Podman @('volume', 'rm', $volume) | Out-Null
        Write-Host "REMOVED $volume"
    }
}
