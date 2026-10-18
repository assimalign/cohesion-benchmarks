#Requires -Version 7.0
<#
.SYNOPSIS
    Moves the pinned cohesion CI build: CohesionPackageVersion in Directory.Build.props (the BenchmarkDotNet
    suites) and the Cohesion SDK pins in benchmarks/resources/Web/global.json (the end-to-end Web harness).

.DESCRIPTION
    Without arguments, pins the newest build of cohesion main. Main builds are pruned from the feed after 20 newer
    ones exist (cohesion docs/VERSIONING_RELEASE_POLICY.md, "Feed growth"), so move the pin at least that often.

.EXAMPLE
    ./scripts/Update-CohesionVersion.ps1                                  # newest main build
    ./scripts/Update-CohesionVersion.ps1 -Version 10.0.0-preview.0.ci.38   # a specific build
#>
[CmdletBinding()]
param(
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$resolve = Join-Path $PSScriptRoot 'Resolve-CohesionVersion.ps1'

$build = if ($Version) { & $resolve -Version $Version } else { & $resolve }
$commit = $build.Commit.Substring(0, 12)
$channel = if ($build.Channel -eq 'pr') { "pull request build, cohesion $commit" } else { "main, cohesion $commit" }

$props = Join-Path $root 'Directory.Build.props'
$text = Get-Content $props -Raw
$updated = $text -replace '(<CohesionPackageVersion Condition="[^"]*">)[^<]+(</CohesionPackageVersion>)', "`${1}$($build.Version)`${2}"
$updated = $updated -replace '<!-- The pinned cohesion CI build \([^)]*\)\.', "<!-- The pinned cohesion CI build ($channel)."
if ($updated -eq $text -and $text -notmatch [regex]::Escape(">$($build.Version)<")) {
    throw "Could not find CohesionPackageVersion in $props."
}
Set-Content -Path $props -Value $updated -NoNewline

$globalJson = Join-Path $root 'benchmarks' 'resources' 'Web' 'global.json'
$json = Get-Content $globalJson -Raw
$pinned = [regex]::Replace($json, '("Assimalign\.Cohesion\.Sdk[^"]*"\s*:\s*")[^"]+(")', "`${1}$($build.Version)`${2}")
Set-Content -Path $globalJson -Value $pinned -NoNewline

Write-Host "Pinned cohesion $($build.Version) ($channel, $($build.RunUrl))."
