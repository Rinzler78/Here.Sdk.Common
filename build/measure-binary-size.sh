#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

MAX_KB=100
MAX_BYTES=$((MAX_KB * 1024))
THRESHOLD_BYTES=$(( MAX_BYTES * 105 / 100 ))  # 5% tolerance = 105 KB

dotnet pack src/Here.Sdk.Premium.Common -c Release -o artifacts/packages --no-build 2>/dev/null \
  || dotnet pack src/Here.Sdk.Premium.Common -c Release -o artifacts/packages

NUPKG=$(find artifacts/packages -name "*.nupkg" | grep -v "\.symbols\." | head -1)
if [ -z "$NUPKG" ]; then
  echo "ERROR: No .nupkg found in artifacts/packages" >&2
  exit 1
fi

SIZE=$(wc -c < "$NUPKG")
SIZE_KB=$(( SIZE / 1024 ))

echo "Package: $(basename "$NUPKG")"
echo "Size:    ${SIZE_KB} KB (${SIZE} bytes)"
echo "Limit:   ${MAX_KB} KB"

if [ "$SIZE" -gt "$THRESHOLD_BYTES" ]; then
  echo "ERROR: Package size ${SIZE_KB} KB exceeds ${MAX_KB} KB limit (with 5% tolerance)" >&2
  exit 1
fi

echo "✓ Binary size check passed"
