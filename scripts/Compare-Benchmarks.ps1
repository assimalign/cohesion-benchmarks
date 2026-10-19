#Requires -Version 7.0
<#
.SYNOPSIS
    Compares two sets of BenchmarkDotNet reports (before/after, main/branch) and prints a markdown table.

.DESCRIPTION
    Matches benchmarks by Type.Method(Params). A benchmark regresses when its median time grows past
    -TimeThreshold or its allocations grow past -AllocationThreshold (ratios, current / baseline). Allocations are
    deterministic, so their default threshold is exact; time is noisy, so judge it against the spread of a repeat run.

.EXAMPLE
    ./scripts/Compare-Benchmarks.ps1 -Baseline .artifacts\ab\before -Current .artifacts\ab\after
    ./scripts/Compare-Benchmarks.ps1 -Baseline before -Current after -TimeThreshold 1.15 -FailOnRegression -OutputPath compare.md
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Baseline,
    [Parameter(Mandatory)][string]$Current,
    [double]$TimeThreshold = 1.10,
    [double]$AllocationThreshold = 1.00,
    [string]$OutputPath,
    [switch]$FailOnRegression
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'BenchmarkReports.psm1') -Force

$before = @{}; Get-BenchmarkResult -Path $Baseline | ForEach-Object { $before[$_.Name] = $_ }
$after = @{}; Get-BenchmarkResult -Path $Current | ForEach-Object { $after[$_.Name] = $_ }

$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add('| Benchmark | Baseline | Current | Time ratio | Alloc baseline | Alloc current | |')
$lines.Add('| --- | ---: | ---: | ---: | ---: | ---: | --- |')

$regressions = 0
foreach ($name in @($before.Keys + $after.Keys | Sort-Object -Unique)) {
    $b = $before[$name]; $c = $after[$name]
    if (-not $b) { $lines.Add("| $name | | $(Format-Duration $c.MedianNs) | | | $($c.Bytes) | new |"); continue }
    if (-not $c) { $lines.Add("| $name | $(Format-Duration $b.MedianNs) | | | $($b.Bytes) | | removed |"); continue }
    if ($b.Failed -or $c.Failed) { $lines.Add("| $name | | | | | | failed in $(if ($b.Failed) { 'baseline' } else { 'current' }) |"); continue }

    $ratio = if ($b.MedianNs -gt 0) { $c.MedianNs / $b.MedianNs } else { [double]::NaN }
    $allocRegressed = ($null -ne $b.Bytes -and $null -ne $c.Bytes) -and (($b.Bytes -eq 0 -and $c.Bytes -gt 0) -or ($b.Bytes -gt 0 -and $c.Bytes / $b.Bytes -gt $AllocationThreshold))
    $timeRegressed = $ratio -gt $TimeThreshold
    $status = if ($timeRegressed -or $allocRegressed) { $regressions++; 'REGRESSION' } elseif ($ratio -lt (1 / $TimeThreshold)) { 'faster' } else { '' }
    $lines.Add("| $name | $(Format-Duration $b.MedianNs) | $(Format-Duration $c.MedianNs) | $('{0:N2}x' -f $ratio) | $($b.Bytes) | $($c.Bytes) | $status |")
}

$lines.Add('')
$lines.Add("$regressions regression(s): time ratio over $TimeThreshold or allocations over $AllocationThreshold x.")
$table = $lines -join [Environment]::NewLine

if ($OutputPath) { Set-Content -Path $OutputPath -Value $table -Encoding utf8 }
Write-Output $table

if ($FailOnRegression -and $regressions -gt 0) { exit 1 }
