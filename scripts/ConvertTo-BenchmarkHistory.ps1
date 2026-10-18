#Requires -Version 7.0
<#
.SYNOPSIS
    Converts BenchmarkDotNet reports into github-action-benchmark's "customSmallerIsBetter" format: one file of
    time series (ns, median) and one of allocation series (bytes per operation).

.DESCRIPTION
    The action's built-in BenchmarkDotNet parser records time only, and keys history to this repository's commit.
    Converting first lets CI track allocations too (deterministic, so they take a tight regression threshold) and
    stamps every data point with the cohesion build it measured (CI package version and commit). Failed benchmarks
    are reported and left out.

.EXAMPLE
    $build = ./scripts/Resolve-CohesionVersion.ps1 -Version 10.0.0-preview.0.ci.38
    ./scripts/ConvertTo-BenchmarkHistory.ps1 -Path results/libraries/Http -TimeOutput time.json -AllocationOutput alloc.json -CohesionVersion $build.Version -CohesionCommit $build.Commit
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Path,
    [Parameter(Mandatory)][string]$TimeOutput,
    [Parameter(Mandatory)][string]$AllocationOutput,
    [string]$CohesionVersion = 'unknown',
    [string]$CohesionCommit = 'unknown',
    [string]$Runner = $env:RUNNER_NAME
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'BenchmarkReports.psm1') -Force

$rows = @(Get-BenchmarkResult -Path $Path)
if ($rows.Count -eq 0) { throw "No BenchmarkDotNet *-report-full.json under $Path." }

$failed = @($rows | Where-Object Failed)
foreach ($row in $failed) { Write-Warning "No statistics for $($row.Name) ($($row.Report)); left out of the history." }

$context = "cohesion $CohesionVersion ($($CohesionCommit.Substring(0, [Math]::Min(12, $CohesionCommit.Length))))" + $(if ($Runner) { " on $Runner" } else { '' })

$time = foreach ($row in $rows | Where-Object { -not $_.Failed }) {
    [ordered]@{
        name  = $row.Name
        unit  = 'ns'
        value = [Math]::Round($row.MedianNs, 3)
        range = '± ' + [Math]::Round($row.StdDevNs, 3)
        extra = "$context`nmean $([Math]::Round($row.MeanNs, 3)) ns"
    }
}

$allocations = foreach ($row in $rows | Where-Object { -not $_.Failed -and $null -ne $_.Bytes }) {
    [ordered]@{
        name  = "$($row.Name) - allocated"
        unit  = 'B'
        value = $row.Bytes
        extra = $context
    }
}

ConvertTo-Json -InputObject @($time) -Depth 4 | Set-Content -Path $TimeOutput -Encoding utf8
ConvertTo-Json -InputObject @($allocations) -Depth 4 | Set-Content -Path $AllocationOutput -Encoding utf8
Write-Host "Wrote $(@($time).Count) time and $(@($allocations).Count) allocation series ($($failed.Count) failed) for $context."
