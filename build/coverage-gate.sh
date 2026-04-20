#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

RESULTS_DIR="artifacts/TestResults"
MIN_LINE=90
MIN_BRANCH=90
MIN_METHOD=95
FAIL=0

# Run tests with coverage if no results exist yet
if ! find "$RESULTS_DIR" -name "coverage.cobertura.xml" 2>/dev/null | grep -q .; then
  dotnet test -c Release \
    --collect:"XPlat Code Coverage" \
    --results-directory "$RESULTS_DIR" \
    -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura
fi

XML=$(find "$RESULTS_DIR" -name "coverage.cobertura.xml" | head -1)
if [ -z "$XML" ]; then
  echo "ERROR: No coverage report found in $RESULTS_DIR" >&2
  exit 1
fi

extract() {
  python3 -c "
import xml.etree.ElementTree as ET, sys
tree = ET.parse('$XML')
root = tree.getroot()
val = root.attrib.get('$1', '0')
print(f'{float(val)*100:.1f}')
"
}

LINE=$(extract "line-rate")
BRANCH=$(extract "branch-rate")
# method coverage from summary
METHOD=$(python3 -c "
import xml.etree.ElementTree as ET
tree = ET.parse('$XML')
root = tree.getroot()
covered = sum(int(m.attrib.get('hits','0')) > 0 for pkg in root.iter('package') for cls in pkg.iter('class') for m in cls.iter('method'))
total = sum(1 for pkg in root.iter('package') for cls in pkg.iter('class') for m in cls.iter('method'))
print(f'{(covered/total*100):.1f}' if total > 0 else '100.0')
")

echo "Coverage summary:"
echo "  Line:   ${LINE}% (min ${MIN_LINE}%)"
echo "  Branch: ${BRANCH}% (min ${MIN_BRANCH}%)"
echo "  Method: ${METHOD}% (min ${MIN_METHOD}%)"

check() {
  local val=$1 min=$2 name=$3
  if python3 -c "import sys; sys.exit(0 if float('$val') >= $min else 1)"; then
    echo "  ✓ $name"
  else
    echo "  ✗ $name BELOW THRESHOLD" >&2
    FAIL=1
  fi
}

check "$LINE"   "$MIN_LINE"   "Line coverage"
check "$BRANCH" "$MIN_BRANCH" "Branch coverage"
check "$METHOD" "$MIN_METHOD" "Method coverage"

[ "$FAIL" -eq 0 ] || { echo "Coverage gate FAILED" >&2; exit 1; }
echo "✓ Coverage gate passed"
