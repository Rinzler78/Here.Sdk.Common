#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "=== [1/7] Format check ==="
dotnet format --verify-no-changes

echo "=== [2/7] Build Release ==="
dotnet build -c Release --no-incremental

echo "=== [3/7] Test ==="
dotnet test -c Release --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory artifacts/TestResults

echo "=== [4/7] Coverage gate ==="
./build/coverage-gate.sh

echo "=== [5/7] Clean architecture check ==="
./build/clean-architecture-check.sh

echo "=== [6/7] OpenSpec lint ==="
./build/openspec-lint.sh

echo "=== [7/7] Detect HERE credentials ==="
./build/detect-here-credentials.sh

echo ""
echo "✓ All checks passed — ready to PR"
