param([Parameter(Mandatory)][string]$Container, [Parameter(Mandatory)][string]$OutputPath)
$ErrorActionPreference = 'Stop'
if (-not ('DbbenchHostClock' -as [type])) {
    Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;
public static class DbbenchHostClock {
    [DllImport("kernel32.dll")] public static extern bool GetSystemTimes(out ulong idle, out ulong kernel, out ulong user);
}
'@
}
[ulong]$idle = 0; [ulong]$kernel = 0; [ulong]$userTime = 0
[DbbenchHostClock]::GetSystemTimes([ref]$idle, [ref]$kernel, [ref]$userTime) | Out-Null
$lastIdle = $idle; $lastTotal = $kernel + $userTime
while ($true) {
    $begun = [DateTime]::UtcNow
    $cpu = $null; $memory = $null; $diagnostic = $null
    $raw = & podman stats --no-stream --format json $Container 2>$null
    if ($LASTEXITCODE -eq 0 -and $raw) {
        $stats = @($raw | ConvertFrom-Json -AsHashtable)[0]
        $cpuText = $stats['cpu_percent']
        if (-not $cpuText) { $cpuText = $stats['cpu'] }
        if ($cpuText) { $cpu = [double]::Parse(([string]$cpuText).TrimEnd('%'), [Globalization.CultureInfo]::InvariantCulture) }
        $memory = $stats['mem_usage']
    } else { $diagnostic = 'podman stats unavailable; container may be starting.' }
    [DbbenchHostClock]::GetSystemTimes([ref]$idle, [ref]$kernel, [ref]$userTime) | Out-Null
    $total = $kernel + $userTime
    $hostCpu = if ($total -gt $lastTotal) { 100.0 * (1.0 - ($idle - $lastIdle) / [double]($total - $lastTotal)) } else { $null }
    $lastIdle = $idle; $lastTotal = $total
    $sample = @{timestamp=[DateTimeOffset]::UtcNow.ToString('O');hostCpuPercent=$hostCpu;containerCpuPercent=$cpu;containerMemory=$memory;diagnostic=$diagnostic} | ConvertTo-Json -Compress
    [IO.File]::WriteAllText($OutputPath, $sample)
    $remaining = 1000 - ([DateTime]::UtcNow - $begun).TotalMilliseconds
    if ($remaining -gt 0) { Start-Sleep -Milliseconds ([int]$remaining) }
}
