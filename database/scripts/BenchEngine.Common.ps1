Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$script:DatabaseRoot = Split-Path $PSScriptRoot -Parent
$script:Catalog = Get-Content -LiteralPath (Join-Path $script:DatabaseRoot 'engines/catalog.json') -Raw | ConvertFrom-Json -AsHashtable
$script:Owner = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($script:DatabaseRoot.ToLowerInvariant()))).Substring(0, 16).ToLowerInvariant()
function Invoke-Podman {
    param([string[]]$PodmanArgs)
    & podman @PodmanArgs
    if ($LASTEXITCODE -ne 0) { throw "podman $($PodmanArgs[0]) failed ($LASTEXITCODE)." }
}
function Get-BenchTarget {
    param([string]$Id, [string]$Suffix = '')
    if ($Suffix -notmatch '^(-[a-z0-9][a-z0-9-]*)?$') { throw 'Suffix must be empty or start with a dash and contain lowercase letters, digits and dashes.' }
    $target = @($script:Catalog.targets | Where-Object { $_.id -eq $Id })
    if ($target.Count -ne 1) { throw "Unknown target: $Id" }
    return $target[0]
}
function Expand-BenchValue {
    param([string]$Value, [hashtable]$Target)
    foreach ($key in $Target.credentials.Keys) { $Value = $Value.Replace("{$key}", [string]$Target.credentials[$key]) }
    return $Value
}
function Assert-BenchOwner {
    param([string]$Name, [switch]$Volume)
    $kind = if ($Volume) { 'volume' } else { 'container' }
    $raw = Invoke-Podman @('inspect', '--type', $kind, $Name)
    $info = @($raw | ConvertFrom-Json -AsHashtable)[0]
    $labels = if ($Volume) { $info.Labels } else { $info.Config.Labels }
    if ($null -eq $labels -or $labels['dbbench.owner'] -ne $script:Owner) { throw "Refusing to change ${Name}: not owned by this benchmarks worktree." }
}
