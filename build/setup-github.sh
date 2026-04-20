#!/usr/bin/env bash
# Idempotent GitHub provisioning for rinzler78/Here.Sdk.Common.
# Re-running against an already-provisioned repo reports "No changes applied".
#
# Prerequisites:
#   - git, gh (authenticated via `gh auth login`)
#   - NUGET_API_KEY env var set (never hardcode it here)
#
# Usage:
#   NUGET_API_KEY=<key> ./build/setup-github.sh
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

REPO="rinzler78/Here.Sdk.Common"
DESCRIPTION="Minimal shared vocabulary for the Here.Sdk.* ecosystem: geography, units, error taxonomy. Zero runtime dependencies."
HOMEPAGE="https://www.nuget.org/packages/Here.Sdk.Common"
TOPICS='["here","maps","nuget","dotnet","xamarin","maui","csharp"]'
CHANGES=0

_changed() { CHANGES=$((CHANGES + 1)); echo "  [+] $*"; }
_skip()    { echo "  [=] $* (unchanged)"; }
_info()    { echo "  [ ] $*"; }

# ── 0. Prerequisites ─────────────────────────────────────────────────────────
echo "=== [0/9] Prerequisites ==="
command -v git >/dev/null 2>&1 || { echo "ERROR: git not found" >&2; exit 1; }
command -v gh  >/dev/null 2>&1 || { echo "ERROR: gh not found — install GitHub CLI" >&2; exit 1; }
gh auth status >/dev/null 2>&1 || { echo "ERROR: not authenticated — run 'gh auth login'" >&2; exit 1; }
if [ -z "${NUGET_API_KEY:-}" ]; then
  echo "ERROR: NUGET_API_KEY env var is not set" >&2
  exit 1
fi
_info "prerequisites ok"

# ── 1. Ensure master branch exists and is pushed ─────────────────────────────
echo "=== [1/9] master branch ==="
if ! git rev-parse --verify master >/dev/null 2>&1; then
  echo "ERROR: local 'master' branch does not exist — run 'git init' + initial commit first" >&2
  exit 1
fi
if ! git ls-remote --exit-code origin refs/heads/master >/dev/null 2>&1; then
  git push origin master
  _changed "pushed master to origin"
else
  _skip "master already on origin"
fi

# ── 2. Create / verify GitHub repository ─────────────────────────────────────
echo "=== [2/9] GitHub repository ==="
if ! gh repo view "$REPO" >/dev/null 2>&1; then
  gh repo create "$REPO" \
    --public \
    --description "$DESCRIPTION" \
    --homepage "$HOMEPAGE" \
    --disable-wiki
  _changed "repository created"
else
  _skip "repository already exists"
fi

# Visibility
vis=$(gh repo view "$REPO" --json visibility --jq '.visibility')
if [ "$vis" != "PUBLIC" ]; then
  gh repo edit "$REPO" --visibility public
  _changed "visibility set to PUBLIC"
else
  _skip "visibility is PUBLIC"
fi

# Description + homepage
cur_desc=$(gh repo view "$REPO" --json description --jq '.description // ""')
cur_home=$(gh repo view "$REPO" --json homepageUrl --jq '.homepageUrl // ""')
if [ "$cur_desc" != "$DESCRIPTION" ] || [ "$cur_home" != "$HOMEPAGE" ]; then
  gh repo edit "$REPO" --description "$DESCRIPTION" --homepage "$HOMEPAGE"
  _changed "description / homepage updated"
else
  _skip "description and homepage"
fi

# Wiki + Discussions
cur_wiki=$(gh repo view "$REPO" --json hasWikiEnabled --jq '.hasWikiEnabled')
cur_disc=$(gh repo view "$REPO" --json hasDiscussionsEnabled --jq '.hasDiscussionsEnabled')
if [ "$cur_wiki" = "true" ]; then
  gh repo edit "$REPO" --enable-wiki=false
  _changed "wiki disabled"
else
  _skip "wiki already disabled"
fi
if [ "$cur_disc" = "false" ]; then
  gh api -X PATCH "repos/$REPO" -F has_discussions=true >/dev/null
  _changed "discussions enabled"
else
  _skip "discussions already enabled"
fi

# ── 3. Topics ─────────────────────────────────────────────────────────────────
echo "=== [3/9] Topics ==="
cur_topics=$(gh repo view "$REPO" --json repositoryTopics --jq '([.repositoryTopics[]?.name] // []) | sort | @json')
want_topics=$(echo "$TOPICS" | python3 -c "import sys,json; t=json.load(sys.stdin); t.sort(); print(json.dumps(t))")
if [ "$cur_topics" != "$want_topics" ]; then
  gh api -X PUT "repos/$REPO/topics" --input <(echo "{\"names\":$(echo "$TOPICS" | python3 -c 'import sys,json;t=json.load(sys.stdin);print(json.dumps(t))')}") >/dev/null
  _changed "topics applied"
else
  _skip "topics already set"
fi

# ── 4. develop branch ─────────────────────────────────────────────────────────
echo "=== [4/9] develop branch ==="
if ! git ls-remote --exit-code origin refs/heads/develop >/dev/null 2>&1; then
  git push origin master:develop
  _changed "develop branch created from master"
else
  _skip "develop already on origin"
fi
cur_default=$(gh repo view "$REPO" --json defaultBranchRef --jq '.defaultBranchRef.name')
if [ "$cur_default" != "develop" ]; then
  gh repo edit "$REPO" --default-branch develop
  _changed "default branch set to develop"
else
  _skip "default branch is develop"
fi

# ── 5. Branch protection Rulesets ─────────────────────────────────────────────
echo "=== [5/9] Branch Rulesets ==="

_apply_ruleset() {
  local name="$1"
  local payload="$2"
  local existing_id
  existing_id=$(gh api "repos/$REPO/rulesets" --jq ".[] | select(.name == \"$name\") | .id" 2>/dev/null || true)
  if [ -z "$existing_id" ]; then
    echo "$payload" | gh api -X POST "repos/$REPO/rulesets" --input - >/dev/null
    _changed "ruleset '$name' created"
  else
    echo "$payload" | gh api -X PUT "repos/$REPO/rulesets/$existing_id" --input - >/dev/null
    _skip "ruleset '$name' already exists (updated)"
  fi
}

STATUS_CHECKS='[{"context":"build (Debug)"},{"context":"build (Release)"},{"context":"openspec-validate / validate"}]'

DEVELOP_RULESET=$(python3 -c "
import json
print(json.dumps({
  'name': 'protect-develop',
  'target': 'branch',
  'enforcement': 'active',
  'conditions': {'ref_name': {'include': ['refs/heads/develop'], 'exclude': []}},
  'rules': [
    {'type': 'deletion'},
    {'type': 'non_fast_forward'},
    {'type': 'required_signatures'},
    {'type': 'required_linear_history'},
    {'type': 'pull_request', 'parameters': {
      'required_approving_review_count': 1,
      'dismiss_stale_reviews_on_push': True,
      'require_code_owner_review': False,
      'require_last_push_approval': True,
      'required_review_thread_resolution': True
    }},
    {'type': 'required_status_checks', 'parameters': {
      'strict_required_status_checks_policy': True,
      'required_status_checks': $STATUS_CHECKS
    }}
  ]
}))
" STATUS_CHECKS="$STATUS_CHECKS")

MASTER_RULESET=$(python3 -c "
import json
print(json.dumps({
  'name': 'protect-master',
  'target': 'branch',
  'enforcement': 'active',
  'conditions': {'ref_name': {'include': ['refs/heads/master'], 'exclude': []}},
  'rules': [
    {'type': 'deletion'},
    {'type': 'non_fast_forward'},
    {'type': 'required_signatures'},
    {'type': 'required_linear_history'},
    {'type': 'pull_request', 'parameters': {
      'required_approving_review_count': 2,
      'dismiss_stale_reviews_on_push': True,
      'require_code_owner_review': False,
      'require_last_push_approval': True,
      'required_review_thread_resolution': True
    }},
    {'type': 'required_status_checks', 'parameters': {
      'strict_required_status_checks_policy': True,
      'required_status_checks': $STATUS_CHECKS
    }}
  ],
  # bypass_actors: add release-please GitHub App ID once installed on the repo
}))
" STATUS_CHECKS="$STATUS_CHECKS")

_apply_ruleset "protect-develop" "$DEVELOP_RULESET"
_apply_ruleset "protect-master"  "$MASTER_RULESET"

# ── 6. Secrets ────────────────────────────────────────────────────────────────
echo "=== [6/9] Secrets ==="
if gh secret list --repo "$REPO" 2>/dev/null | grep -q "^NUGET_API_KEY"; then
  _skip "NUGET_API_KEY already registered"
else
  echo "$NUGET_API_KEY" | gh secret set NUGET_API_KEY --repo "$REPO" --body -
  _changed "NUGET_API_KEY registered"
fi

# ── 7. nuget-production Environment ──────────────────────────────────────────
echo "=== [7/9] Environment: nuget-production ==="
env_exists=$(gh api "repos/$REPO/environments/nuget-production" --jq '.name' 2>/dev/null || true)
maintainer_id=$(gh api "users/rinzler78" --jq '.id' 2>/dev/null || echo "0")

ENV_PAYLOAD=$(python3 -c "
import json
print(json.dumps({
  'wait_timer': 0,
  'reviewers': [{'type': 'User', 'id': int('$maintainer_id')}],
  'deployment_branch_policy': {
    'protected_branches': False,
    'custom_branch_policies': True
  }
}))
" maintainer_id="$maintainer_id")

if [ -z "$env_exists" ]; then
  echo "$ENV_PAYLOAD" | gh api -X PUT "repos/$REPO/environments/nuget-production" --input - >/dev/null
  _changed "nuget-production environment created"
else
  echo "$ENV_PAYLOAD" | gh api -X PUT "repos/$REPO/environments/nuget-production" --input - >/dev/null
  _skip "nuget-production already exists (updated)"
fi

# Deployment branch policy: master only
gh api -X POST "repos/$REPO/environments/nuget-production/deployment-branch-policies" \
  -f name="master" >/dev/null 2>&1 || true

# ── 8. Dependabot ─────────────────────────────────────────────────────────────
echo "=== [8/9] Dependabot ==="
DEPENDABOT=".github/dependabot.yml"
if [ ! -f "$DEPENDABOT" ]; then
  cat > "$DEPENDABOT" <<'YAML'
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
YAML
  _changed "dependabot.yml created (commit and push)"
else
  _skip "dependabot.yml already present"
fi

# ── 9. Summary ────────────────────────────────────────────────────────────────
echo "=== [9/9] Summary ==="
if [ "$CHANGES" -eq 0 ]; then
  echo "No changes applied."
else
  echo "$CHANGES change(s) applied."
fi
