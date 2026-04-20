#Requires -Version 7
# Idempotent GitHub provisioning for rinzler78/Here.Sdk.Common.
# Re-running against an already-provisioned repo reports "No changes applied".
#
# Prerequisites:
#   - git, gh (authenticated via `gh auth login`)
#   - $env:NUGET_API_KEY set (never hardcode it here)
#
# Usage:
#   $env:NUGET_API_KEY = "<key>"; .\build\setup-github.ps1

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$Repo        = 'rinzler78/Here.Sdk.Common'
$Description = 'Minimal shared vocabulary for the Here.Sdk.* ecosystem: geography, units, error taxonomy. Zero runtime dependencies.'
$Homepage    = 'https://www.nuget.org/packages/Here.Sdk.Common'
$Topics      = @('here','maps','nuget','dotnet','xamarin','maui','csharp')
$Changes     = 0

function Write-Changed([string]$msg) { $script:Changes++; Write-Host "  [+] $msg" }
function Write-Skip([string]$msg)    { Write-Host "  [=] $msg (unchanged)" }
function Write-Info([string]$msg)    { Write-Host "  [ ] $msg" }

# ── 0. Prerequisites ──────────────────────────────────────────────────────────
Write-Host '=== [0/9] Prerequisites ==='
foreach ($cmd in 'git','gh') {
    if (-not (Get-Command $cmd -ErrorAction SilentlyContinue)) {
        Write-Error "$cmd not found"; exit 1
    }
}
gh auth status 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Error 'Not authenticated — run gh auth login'; exit 1 }
if (-not $env:NUGET_API_KEY) { Write-Error 'NUGET_API_KEY env var not set'; exit 1 }
Write-Info 'prerequisites ok'

# ── 1. master branch ──────────────────────────────────────────────────────────
Write-Host '=== [1/9] master branch ==='
$masterExists = git rev-parse --verify master 2>&1
if ($LASTEXITCODE -ne 0) { Write-Error "local 'master' branch does not exist"; exit 1 }
$remotemaster = git ls-remote origin refs/heads/master 2>&1
if (-not $remotemaster) {
    git push origin master
    Write-Changed 'pushed master to origin'
} else { Write-Skip 'master already on origin' }

# ── 2. Repository ─────────────────────────────────────────────────────────────
Write-Host '=== [2/9] GitHub repository ==='
$repoJson = gh repo view $Repo --json visibility,description,homepageUrl,hasWikiEnabled,hasDiscussionsEnabled 2>&1
if ($LASTEXITCODE -ne 0) {
    gh repo create $Repo --public --description $Description --homepage $Homepage --disable-wiki
    Write-Changed 'repository created'
    $repoJson = gh repo view $Repo --json visibility,description,homepageUrl,hasWikiEnabled,hasDiscussionsEnabled
}
$repo = $repoJson | ConvertFrom-Json

if ($repo.visibility -ne 'PUBLIC') {
    gh repo edit $Repo --visibility public; Write-Changed 'visibility set to PUBLIC'
} else { Write-Skip 'visibility is PUBLIC' }

if ($repo.description -ne $Description -or $repo.homepageUrl -ne $Homepage) {
    gh repo edit $Repo --description $Description --homepage $Homepage
    Write-Changed 'description / homepage updated'
} else { Write-Skip 'description and homepage' }

if ($repo.hasWikiEnabled) {
    gh repo edit $Repo --enable-wiki=false; Write-Changed 'wiki disabled'
} else { Write-Skip 'wiki already disabled' }

if (-not $repo.hasDiscussionsEnabled) {
    gh api -X PATCH "repos/$Repo" -F has_discussions=true | Out-Null
    Write-Changed 'discussions enabled'
} else { Write-Skip 'discussions already enabled' }

# ── 3. Topics ─────────────────────────────────────────────────────────────────
Write-Host '=== [3/9] Topics ==='
$curTopics = (gh repo view $Repo --json repositoryTopics | ConvertFrom-Json).repositoryTopics.name | Sort-Object
$wantTopics = $Topics | Sort-Object
if (-not ($curTopics -and (Compare-Object $curTopics $wantTopics -SyncWindow 0).Count -eq 0)) {
    $body = @{ names = $Topics } | ConvertTo-Json -Compress
    gh api -X PUT "repos/$Repo/topics" --input ([System.IO.MemoryStream]::new([System.Text.Encoding]::UTF8.GetBytes($body))) | Out-Null
    Write-Changed 'topics applied'
} else { Write-Skip 'topics already set' }

# ── 4. develop branch ─────────────────────────────────────────────────────────
Write-Host '=== [4/9] develop branch ==='
$remoteDevelop = git ls-remote origin refs/heads/develop 2>&1
if (-not $remoteDevelop) {
    git push origin master:develop; Write-Changed 'develop created from master'
} else { Write-Skip 'develop already on origin' }
$defaultBranch = (gh repo view $Repo --json defaultBranchRef | ConvertFrom-Json).defaultBranchRef.name
if ($defaultBranch -ne 'develop') {
    gh repo edit $Repo --default-branch develop; Write-Changed 'default branch set to develop'
} else { Write-Skip 'default branch is develop' }

# ── 5. Rulesets ───────────────────────────────────────────────────────────────
Write-Host '=== [5/9] Branch Rulesets ==='

function Apply-Ruleset([string]$name, [object]$payload) {
    $existing = gh api "repos/$Repo/rulesets" | ConvertFrom-Json | Where-Object { $_.name -eq $name }
    $body = $payload | ConvertTo-Json -Depth 10 -Compress
    if (-not $existing) {
        $body | gh api -X POST "repos/$Repo/rulesets" --input - | Out-Null
        Write-Changed "ruleset '$name' created"
    } else {
        $body | gh api -X PUT "repos/$Repo/rulesets/$($existing.id)" --input - | Out-Null
        Write-Skip "ruleset '$name' already exists (updated)"
    }
}

$statusChecks = @(
    @{context='build (Debug)'},
    @{context='build (Release)'},
    @{context='lint'}
)
$commonRules = @(
    @{type='deletion'},
    @{type='non_fast_forward'},
    @{type='required_signatures'},
    @{type='required_linear_history'},
    @{type='required_status_checks'; parameters=@{
        strict_required_status_checks_policy=$true
        required_status_checks=$statusChecks
    }}
)

Apply-Ruleset 'protect-develop' @{
    name='protect-develop'; target='branch'; enforcement='active'
    conditions=@{ref_name=@{include=@('refs/heads/develop'); exclude=@()}}
    rules = $commonRules + @(@{type='pull_request'; parameters=@{
        required_approving_review_count=1
        dismiss_stale_reviews_on_push=$true
        require_last_push_approval=$true
        required_review_thread_resolution=$true
    }})
}

Apply-Ruleset 'protect-master' @{
    name='protect-master'; target='branch'; enforcement='active'
    conditions=@{ref_name=@{include=@('refs/heads/master'); exclude=@()}}
    rules = $commonRules + @(@{type='pull_request'; parameters=@{
        required_approving_review_count=2
        dismiss_stale_reviews_on_push=$true
        require_last_push_approval=$true
        required_review_thread_resolution=$true
    }})
    bypass_actors=@(@{actor_id=0; actor_type='Integration'; bypass_mode='always'})
}

# ── 6. Secrets ────────────────────────────────────────────────────────────────
Write-Host '=== [6/9] Secrets ==='
$secretList = gh secret list --repo $Repo 2>&1
if ($secretList -match 'NUGET_API_KEY') {
    Write-Skip 'NUGET_API_KEY already registered'
} else {
    $env:NUGET_API_KEY | gh secret set NUGET_API_KEY --repo $Repo --body -
    Write-Changed 'NUGET_API_KEY registered'
}

# ── 7. nuget-production Environment ──────────────────────────────────────────
Write-Host '=== [7/9] Environment: nuget-production ==='
$maintainerId = (gh api "users/rinzler78" | ConvertFrom-Json).id
$envPayload = @{
    wait_timer=0
    reviewers=@(@{type='User'; id=$maintainerId})
    deployment_branch_policy=@{protected_branches=$false; custom_branch_policies=$true}
} | ConvertTo-Json -Depth 5 -Compress

$existing = gh api "repos/$Repo/environments/nuget-production" 2>&1
$created = $LASTEXITCODE -ne 0
$envPayload | gh api -X PUT "repos/$Repo/environments/nuget-production" --input - | Out-Null
if ($created) { Write-Changed 'nuget-production created' } else { Write-Skip 'nuget-production already exists (updated)' }
gh api -X POST "repos/$Repo/environments/nuget-production/deployment-branch-policies" -f name='master' 2>&1 | Out-Null

# ── 8. Dependabot ─────────────────────────────────────────────────────────────
Write-Host '=== [8/9] Dependabot ==='
$dependabotPath = Join-Path $PSScriptRoot '..\.github\dependabot.yml'
if (-not (Test-Path $dependabotPath)) {
    @"
version: 2
updates:
  - package-ecosystem: nuget
    directory: "/"
    schedule:
      interval: weekly
    open-pull-requests-limit: 5
  - package-ecosystem: github-actions
    directory: "/"
    schedule:
      interval: weekly
    open-pull-requests-limit: 5
"@ | Set-Content $dependabotPath
    Write-Changed 'dependabot.yml created (commit and push)'
} else { Write-Skip 'dependabot.yml already present' }

# ── 9. Summary ────────────────────────────────────────────────────────────────
Write-Host '=== [9/9] Summary ==='
if ($Changes -eq 0) { Write-Host 'No changes applied.' } else { Write-Host "$Changes change(s) applied." }
