#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
FAIL=0

check_file() {
  if [ ! -f "$1" ]; then
    echo "MISSING: $1" >&2
    FAIL=1
  fi
}

# Required OpenSpec root files
check_file "$ROOT/openspec/project.md"
check_file "$ROOT/openspec/tech.md"
check_file "$ROOT/openspec/README.md"
check_file "$ROOT/openspec/AGENTS.md"

# Validate in-flight proposals
for proposal in "$ROOT/openspec/changes"/*/proposal.md; do
  [ -f "$proposal" ] || continue
  dir=$(dirname "$proposal")
  slug=$(basename "$dir")

  # Required frontmatter fields
  for field in id title status author created semver-impact; do
    if ! grep -q "^${field}:" "$proposal" 2>/dev/null; then
      echo "FRONTMATTER_MISSING: $proposal missing '$field'" >&2
      FAIL=1
    fi
  done

  # Required sections
  for section in "## Why" "## What changes" "## Impact" "## Alternatives considered"; do
    if ! grep -qF "$section" "$proposal" 2>/dev/null; then
      echo "SECTION_MISSING: $proposal missing '$section'" >&2
      FAIL=1
    fi
  done

  # tasks.md required
  if [ ! -f "$dir/tasks.md" ]; then
    echo "MISSING_TASKS: $dir/tasks.md not found" >&2
    FAIL=1
  fi
done

# Validate delta specs
for spec in "$ROOT/openspec/changes"/*/specs/*/spec.md; do
  [ -f "$spec" ] || continue
  if ! grep -qE "^## (ADDED|MODIFIED|REMOVED|RENAMED) Requirements" "$spec"; then
    echo "SPEC_MISSING_DELTA_SECTION: $spec has no ADDED/MODIFIED/REMOVED/RENAMED section" >&2
    FAIL=1
  fi
  # Every Requirement must have a Scenario
  while IFS= read -r req; do
    lineno=$(grep -n "^### Requirement:" "$spec" | grep -F "$req" | head -1 | cut -d: -f1)
    # Check there's a Scenario somewhere after
    if ! awk "NR>$lineno && /^#### Scenario:/{found=1; exit} NR>$lineno && /^### Requirement:/{exit} END{exit !found}" "$spec" 2>/dev/null; then
      echo "REQUIREMENT_WITHOUT_SCENARIO: $spec — '$req'" >&2
      FAIL=1
    fi
  done < <(grep "^### Requirement:" "$spec" | sed 's/^### Requirement: //')
done

[ "$FAIL" -eq 0 ] || { echo "OpenSpec lint FAILED" >&2; exit 1; }
echo "✓ OpenSpec lint passed"
