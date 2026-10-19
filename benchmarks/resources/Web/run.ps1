#Requires -Version 7.0
<#
.SYNOPSIS
    Publishes the Cohesion and ASP.NET Core scenario servers and runs the Web benchmark against both.

.DESCRIPTION
    Both servers are published Release, self-contained for the host RID, into artifacts/publish/<framework>,
    then the runner starts one server process per scenario, checks its response, and measures it.
    Results land in results/<timestamp>/results.{md,json} unless -Output says otherwise.

    The Cohesion SDK and packages are the cohesion CI build pinned in global.json, restored from the Assimalign
    GitHub Packages feed (repository nuget.config). Set the feed credential first:

        $env:NuGetPackageSourceCredentials_assimalign = "Username=<github-user>;Password=$(gh auth token)"

    The token needs read:packages (gh auth refresh -h github.com -s read:packages). Move the pin with
    ../../../scripts/Update-CohesionVersion.ps1.

    Arguments after the named parameters pass straight to the runner (see `-- --help`).

.EXAMPLE
    ./run.ps1
        Every scenario, default load (64 connections, 5 s warmup, 10 s measured).

.EXAMPLE
    ./run.ps1 -Scenarios plaintext,json,full-stack -Iterations 3
        Three scenarios, three runs each, medians reported.

.EXAMPLE
    ./run.ps1 -ValidateOnly
        Start each server once per scenario and record what it answers; no load.
#>
[CmdletBinding()]
param(
    [string]$Scenarios = 'all',
    [int]$Connections = 64,
    [int]$Warmup = 5,
    [int]$Duration = 10,
    [int]$Iterations = 1,
    [string]$Output,
    [string]$Label,
    [switch]$ValidateOnly,
    [switch]$Aot,
    [switch]$SkipPublish,
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$RunnerArguments
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$publish = Join-Path $root 'artifacts' 'publish'
$runnerProject = Join-Path $root 'benchmarks/Assimalign.Cohesion.Web.Benchmarks.Runner/src/Assimalign.Cohesion.Web.Benchmarks.Runner.csproj'

$servers = [ordered]@{
    cohesion   = Join-Path $root 'benchmarks/Assimalign.Cohesion.Web.Benchmarks.CohesionServer/src/Assimalign.Cohesion.Web.Benchmarks.CohesionServer.csproj'
    aspnetcore = Join-Path $root 'benchmarks/Assimalign.Cohesion.Web.Benchmarks.AspNetCoreServer/src/Assimalign.Cohesion.Web.Benchmarks.AspNetCoreServer.csproj'
}

if (-not $SkipPublish) {
    foreach ($framework in $servers.Keys) {
        $out = Join-Path $publish $framework
        if (Test-Path $out) { Remove-Item $out -Recurse -Force }
        $publishArgs = @('publish', $servers[$framework], '-c', 'Release', '-o', $out, '--nologo', '-v', 'quiet')
        if ($Aot) { $publishArgs += '-p:PublishAot=true' }
        Write-Host "Publishing $framework server -> $out"
        & dotnet @publishArgs
        if ($LASTEXITCODE -ne 0) { throw "Publishing the $framework server failed." }
    }
}

& dotnet build $runnerProject -c Release --nologo -v quiet
if ($LASTEXITCODE -ne 0) { throw 'Building the runner failed.' }

# The report records the cohesion commit behind the pinned CI build, when the GitHub CLI can look it up.
$cohesionVersion = (Get-Content (Join-Path $root 'global.json') -Raw | ConvertFrom-Json).'msbuild-sdks'.'Assimalign.Cohesion.Sdk.Web'
$commit = try {
    (& (Join-Path $root '..' '..' '..' 'scripts' 'Resolve-CohesionVersion.ps1') -Version $cohesionVersion).Commit.Substring(0, 12)
}
catch {
    Write-Warning "Could not look up the commit behind cohesion $($cohesionVersion): $($_.Exception.Message)"
    $null
}

$runArgs = @(
    '--scenarios', $Scenarios,
    '--connections', $Connections,
    '--warmup', $Warmup,
    '--duration', $Duration,
    '--iterations', $Iterations
)
if ($Output) { $runArgs += @('--output', $Output) }
if ($Label) { $runArgs += @('--label', $Label) }
if ($commit) { $runArgs += @('--cohesion-commit', $commit) }
if ($ValidateOnly) { $runArgs += '--validate-only' }
if ($RunnerArguments) { $runArgs += $RunnerArguments }

& dotnet run --project $runnerProject -c Release --no-build -- @runArgs
exit $LASTEXITCODE
