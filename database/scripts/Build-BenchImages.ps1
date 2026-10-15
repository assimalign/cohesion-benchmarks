[CmdletBinding()]
param([ValidatePattern('^[a-zA-Z0-9_.-]+$')][string]$Tag = 'dev')
. (Join-Path $PSScriptRoot 'BenchEngine.Common.ps1')
if (-not $env:COHESION_ROOT) { throw 'Set COHESION_ROOT to the pinned read-only Cohesion worktree.' }
$benchmarkCommit = (& git -C $script:DatabaseRoot rev-parse HEAD).Trim()
$cohesionCommit = (& git -C $env:COHESION_ROOT rev-parse HEAD).Trim()
$buildRoot = Join-Path $script:DatabaseRoot 'artifacts/images'
New-Item -ItemType Directory -Path $buildRoot -Force | Out-Null
Push-Location $script:DatabaseRoot
try {
    foreach ($image in @(@{Project='Cli'; Name='driver'}, @{Project='CohesionServer'; Name='cohesion'})) {
        $context = Join-Path $buildRoot $image.Name
        $publish = Join-Path $context 'publish'
        New-Item -ItemType Directory -Path $publish -Force | Out-Null
        $project = "src/Assimalign.Cohesion.Benchmarks.Database.$($image.Project)/Assimalign.Cohesion.Benchmarks.Database.$($image.Project).csproj"
        & dotnet publish $project -c Release -r linux-arm64 --self-contained true -p:ShouldUnsetParentConfigurationAndPlatform=false -o $publish --nologo
        if ($LASTEXITCODE -ne 0) { throw "Publish failed: $project" }
        if ($image.Name -eq 'driver') {
            Copy-Item -LiteralPath (Join-Path $script:DatabaseRoot 'engines') -Destination $context -Recurse -Force
        }
        $imageName = if ($image.Name -eq 'driver') { 'driver' } else { 'cohesion-server' }
        Invoke-Podman @('build', '--tag', "localhost/dbbench-${imageName}:$Tag", '--build-arg', "BENCHMARKS_COMMIT=$benchmarkCommit",
            '--build-arg', "COHESION_COMMIT=$cohesionCommit", '--file', (Join-Path $script:DatabaseRoot "engines/$($image.Name)/Containerfile"), $context)
    }
} finally { Pop-Location }
Write-Host "BUILT localhost/dbbench-driver:$Tag localhost/dbbench-cohesion-server:$Tag cohesion=$cohesionCommit benchmarks=$benchmarkCommit"
