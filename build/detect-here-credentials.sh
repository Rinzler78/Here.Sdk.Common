#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
FAIL=0

PATTERNS=(
  "ACCESS_KEY_ID=[A-Za-z0-9+/=_-]{20,}"
  "ACCESS_KEY_SECRET=[A-Za-z0-9+/=_-]{20,}"
  "apiKey=[A-Za-z0-9_-]{30,}"
  "HERE_API_KEY"
  "hereApiKey"
)

# build/ excluded: the script itself contains the patterns it searches for
SEARCH_DIRS=("$ROOT/src" "$ROOT/tests" "$ROOT/.github")

for dir in "${SEARCH_DIRS[@]}"; do
  [ -d "$dir" ] || continue
  for pattern in "${PATTERNS[@]}"; do
    if grep -rE "$pattern" "$dir" --include="*.cs" --include="*.sh" --include="*.yml" --include="*.yaml" --include="*.json" -l 2>/dev/null | grep -v ".secrets.baseline" | grep -q .; then
      echo "ERROR: HERE credential pattern '$pattern' found in $dir" >&2
      grep -rE "$pattern" "$dir" --include="*.cs" --include="*.sh" --include="*.yml" --include="*.yaml" --include="*.json" -l 2>/dev/null >&2
      FAIL=1
    fi
  done
done

[ "$FAIL" -eq 0 ] || { echo "Credential scan FAILED" >&2; exit 1; }
echo "✓ No HERE credentials found"
