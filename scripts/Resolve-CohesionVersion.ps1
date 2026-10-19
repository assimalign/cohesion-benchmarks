#Requires -Version 7.0
<#
.SYNOPSIS
    Finds a cohesion CI package build on the Assimalign GitHub Packages feed: the newest build of main, the newest
    build of a cohesion pull request, or the commit behind a given version.

.DESCRIPTION
    cohesion's "CI Packages" workflow (.github/workflows/ci-packages.yml) publishes the full release set for every
    main push as 10.0.0-preview.<n>.ci.<run> and for every same-repository pull request commit as
    10.0.0-preview.<n>.pr.<pr>.<run>. Its publish job is named "publish <version> to GitHub Packages" and its run
    records the commit it packed, so the workflow's public run history answers which builds exist without any
    package-registry permission. Requires the GitHub CLI (gh); in CI it authenticates with GH_TOKEN.

    Builds are pruned: a pull request's builds when it closes, and main builds outside the newest 20.

.OUTPUTS
    An object with Version, Commit, Run, Channel ('ci' or 'pr') and RunUrl. -AsJson writes it as compressed JSON.

.EXAMPLE
    ./scripts/Resolve-CohesionVersion.ps1                       # newest main build
    ./scripts/Resolve-CohesionVersion.ps1 -PullRequest 1663     # newest build of a cohesion pull request
    ./scripts/Resolve-CohesionVersion.ps1 -Version 10.0.0-preview.0.ci.38   # the commit behind a version
#>
[CmdletBinding(DefaultParameterSetName = 'Main')]
param(
    [Parameter(ParameterSetName = 'PullRequest', Mandatory)]
    [int]$PullRequest,

    [Parameter(ParameterSetName = 'Version', Mandatory)]
    [string]$Version,

    [string]$Repository = 'assimalign/cohesion',

    [switch]$AsJson
)

$ErrorActionPreference = 'Stop'
$workflow = 'ci-packages.yml'
$publishJob = '^publish (?<version>\S+) to GitHub Packages$'

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    throw 'The GitHub CLI (gh) is required to look up cohesion CI builds.'
}

function Invoke-GitHub([string]$Path) {
    $json = & gh api $Path
    if ($LASTEXITCODE -ne 0) { throw "gh api $Path failed." }
    return $json | ConvertFrom-Json -Depth 32
}

# The version a successful run published, from its publish job's name; $null when it published nothing.
function Get-PublishedVersion($Run) {
    $jobs = Invoke-GitHub "repos/$Repository/actions/runs/$($Run.id)/jobs?per_page=100"
    foreach ($job in $jobs.jobs) {
        if ($job.conclusion -eq 'success' -and $job.name -match $publishJob) {
            return $Matches.version
        }
    }

    return $null
}

function New-Result([string]$PublishedVersion, $Run) {
    [pscustomobject]@{
        Version = $PublishedVersion
        Commit  = $Run.head_sha
        Run     = $Run.run_number
        Channel = if ($PublishedVersion -match '\.pr\.\d+\.\d+$') { 'pr' } else { 'ci' }
        RunUrl  = $Run.html_url
    }
}

function Write-Result($Result) {
    if ($AsJson) { $Result | ConvertTo-Json -Compress } else { $Result }
}

switch ($PSCmdlet.ParameterSetName) {
    'Main' {
        $runs = Invoke-GitHub "repos/$Repository/actions/workflows/$workflow/runs?branch=main&event=push&status=success&per_page=20"
        foreach ($run in $runs.workflow_runs) {
            $published = Get-PublishedVersion $run
            if ($published) { return Write-Result (New-Result $published $run) }
        }

        throw "No successful main build of $Repository's $workflow published packages in its last 20 runs."
    }

    'PullRequest' {
        for ($page = 1; $page -le 5; $page++) {
            $runs = Invoke-GitHub "repos/$Repository/actions/workflows/$workflow/runs?event=pull_request&status=success&per_page=100&page=$page"
            foreach ($run in $runs.workflow_runs) {
                # GitHub links a run to its pull request only while the pull request is open; after it closes the
                # list is empty, so fall back to the pull request number inside the published version.
                $linked = @($run.pull_requests | Where-Object number -eq $PullRequest).Count -gt 0
                if (-not $linked -and @($run.pull_requests).Count -gt 0) { continue }
                $published = Get-PublishedVersion $run
                if ($published -and $published -match "\.pr\.$PullRequest\.\d+$") {
                    if (-not $linked) {
                        Write-Warning "Pull request #$PullRequest is closed; its builds are pruned when it closes, so $published may no longer restore."
                    }

                    return Write-Result (New-Result $published $run)
                }
            }

            if ($runs.workflow_runs.Count -lt 100) { break }
        }

        throw "No published build of $Repository pull request #$PullRequest was found. Builds of a closed pull request are pruned."
    }

    'Version' {
        if ($Version -notmatch '\.(ci|pr\.\d+)\.(?<run>\d+)$') {
            throw "'$Version' is not a cohesion CI package version (<base>.ci.<run> or <base>.pr.<pr>.<run>)."
        }

        $runNumber = [int]$Matches.run
        for ($page = 1; $page -le 10; $page++) {
            $runs = Invoke-GitHub "repos/$Repository/actions/workflows/$workflow/runs?per_page=100&page=$page"
            $run = $runs.workflow_runs | Where-Object run_number -eq $runNumber | Select-Object -First 1
            if ($run) {
                $published = Get-PublishedVersion $run
                if ($published -ne $Version) {
                    throw "Run $runNumber of $workflow published '$published', not '$Version'."
                }

                return Write-Result (New-Result $published $run)
            }

            if ($runs.workflow_runs.Count -lt 100) { break }
        }

        throw "Run $runNumber of $Repository's $workflow was not found."
    }
}
