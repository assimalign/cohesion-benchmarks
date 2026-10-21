#Requires -Version 7.0
<#
.SYNOPSIS
    Finds the BenchmarkDotNet suites by convention and runs them.

.DESCRIPTION
    A suite is benchmarks/<libraries|resources>/<Area>/benchmarks/<Name>/src/<Name>.csproj where <Name> ends in
    .Benchmarks (CONVENTIONS.md). Each suite is built once (Release) against a cohesion CI build from the Assimalign
    GitHub Packages feed (the pinned CohesionPackageVersion unless -CohesionVersion names another) and run under a
    profile; its BenchmarkDotNet reports land in .artifacts/results/<Name>/ and, with -Output, are copied to
    <Output>/<scope>/<Area>/.

    The feed needs a token with read:packages, supplied through NuGetPackageSourceCredentials_assimalign.

.EXAMPLE
    ./scripts/Invoke-Benchmarks.ps1 -Area ObjectPool
    ./scripts/Invoke-Benchmarks.ps1 -Scope libraries -Profile dry
    ./scripts/Invoke-Benchmarks.ps1 -Area Http -Profile ci -CohesionVersion 10.0.0-preview.0.pr.1663.37 -Output .artifacts\ab\after
    ./scripts/Invoke-Benchmarks.ps1 -List -AsJson        # the CI matrix
#>
[CmdletBinding()]
param(
    [ValidateSet('all', 'libraries', 'resources')]
    [string]$Scope = 'all',

    # Area folder names (ObjectPool, Http, Web, ...). Default: every area in scope.
    [string[]]$Area,

    # BenchmarkDotNet --filter glob.
    [string]$Filter = '*',

    [ValidateSet('local', 'ci', 'dry')]
    [string]$Profile = 'local',

    # jit measures in-process, the whole suite. aot natively compiles and measures only the cases marked
    # [BenchmarkCategory('Aot')] -- NativeAOT compiles one project per case, so measuring them all is not viable.
    # A suite with no marked case reports nothing on aot and still succeeds.
    [ValidateSet('jit', 'aot')]
    [string]$Runtime = 'jit',

    # The cohesion CI package version to measure (scripts/Resolve-CohesionVersion.ps1 finds one). Default: the pin
    # in Directory.Build.props.
    [string]$CohesionVersion,

    # The framework to measure on. Only the monikers cohesion ships resolve (net10.0 today); see
    # BenchmarkTargetFrameworks in Directory.Build.props.
    [string]$Tfm,

    # Copy each suite's reports here, under <scope>/<Area>/.
    [string]$Output,

    [switch]$List,
    [switch]$AsJson,
    [switch]$NoBuild,

    # Anything else goes to BenchmarkDotNet (for example --out-of-process, --memoryRandomization).
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$BenchmarkArguments
)

$ErrorActionPreference = 'Stop'
$env:MSBUILDDISABLENODEREUSE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

$root = Split-Path $PSScriptRoot -Parent
# pwsh -File passes "A,B" as one string; accept both forms.
$Area = @($Area | ForEach-Object { $_ -split ',' } | ForEach-Object Trim | Where-Object { $_ })
$scopes = if ($Scope -eq 'all') { @('libraries', 'resources') } else { @($Scope) }

$suites = foreach ($s in $scopes) {
    $areaRoot = Join-Path $root 'benchmarks' $s
    if (-not (Test-Path $areaRoot)) { continue }
    Get-ChildItem $areaRoot -Directory | Sort-Object Name | ForEach-Object {
        $areaName = $_.Name
        if ($Area -and $Area -notcontains $areaName) { return }
        Get-ChildItem (Join-Path $_.FullName 'benchmarks') -Directory -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -like '*.Benchmarks' } |
            ForEach-Object {
                $project = Join-Path $_.FullName 'src' "$($_.Name).csproj"
                if (Test-Path $project) {
                    [pscustomobject]@{
                        scope   = $s
                        area    = $areaName
                        name    = $_.Name
                        project = [IO.Path]::GetRelativePath($root, $project).Replace('\', '/')
                    }
                }
            }
    }
}

if ($List) {
    if ($AsJson) { ConvertTo-Json -InputObject @($suites) -Compress } else { $suites | Format-Table -AutoSize | Out-String | Write-Host }
    return
}

if (-not $suites) { throw "No benchmark suite matches scope '$Scope' and area '$($Area -join ',')'." }

$msbuildArgs = @('-c', 'Release', '--nologo', '-v', 'quiet')
if ($CohesionVersion) {
    $msbuildArgs += "-p:CohesionPackageVersion=$CohesionVersion"
    Write-Host "Cohesion: $CohesionVersion"
}
# The property drives TargetFramework, so build and run have to agree on it or `dotnet run --no-build` looks in
# the wrong output folder.
$runArgs = @()
if ($Tfm) {
    $msbuildArgs += "-p:BenchmarkTargetFramework=$Tfm"
    $runArgs += "-p:BenchmarkTargetFramework=$Tfm"
    Write-Host "Framework: $Tfm"
}

if (-not $env:NuGetPackageSourceCredentials_assimalign) {
    Write-Warning ("NuGetPackageSourceCredentials_assimalign is not set; restoring Assimalign.Cohesion.* from GitHub Packages " +
        "needs a token with read:packages: `$env:NuGetPackageSourceCredentials_assimalign = `"Username=<github-user>;Password=`$(gh auth token)`"")
}

$failed = @()
foreach ($suite in $suites) {
    $project = Join-Path $root $suite.project
    Write-Host ''
    Write-Host "=== $($suite.scope)/$($suite.area): $($suite.name) [$Profile/$Runtime]" -ForegroundColor Cyan

    if (-not $NoBuild) {
        & dotnet build $project @msbuildArgs
        if ($LASTEXITCODE -ne 0) { $failed += "$($suite.name) (build)"; continue }
    }

    # The harness gives each runtime its own artifacts folder, so a jit and an aot run of one suite coexist.
    $results = Join-Path $root '.artifacts' 'results' ($Runtime -eq 'jit' ? $suite.name : "$($suite.name).$Runtime")
    if (Test-Path $results) { Remove-Item $results -Recurse -Force }

    # "$Filter" is quoted on purpose: on Linux and macOS PowerShell expands an unquoted wildcard argument to a native
    # command into file names, so a bare $Filter of '*' reached BenchmarkDotNet as the repository's file list.
    & dotnet run --project $project -c Release --no-build @runArgs -- --profile $Profile --runtime $Runtime --filter "$Filter" @BenchmarkArguments
    if ($LASTEXITCODE -ne 0) { $failed += "$($suite.name) (run)" }

    if ($Output -and (Test-Path $results)) {
        $destination = Join-Path $Output $suite.scope $suite.area
        New-Item -ItemType Directory -Force -Path $destination | Out-Null
        Get-ChildItem $results -Recurse -File -Include '*-report-full.json', '*-report-github.md' |
            Copy-Item -Destination $destination -Force
    }
}

Write-Host ''
if ($failed) {
    Write-Host "Failed: $($failed -join ', ')" -ForegroundColor Red
    exit 1
}

Write-Host "All $(@($suites).Count) suite(s) ran." -ForegroundColor Green
