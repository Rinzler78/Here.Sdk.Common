#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

RESULTS_DIR="artifacts/TestResults/coverage-gate"
MIN_LINE=90
MIN_BRANCH=90
MIN_METHOD=95
FAIL=0

# Always regenerate to ensure exclusions are applied correctly
rm -rf "$RESULTS_DIR"
dotnet test -c Release \
  --collect:"XPlat Code Coverage" \
  --results-directory "$RESULTS_DIR" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura \
     DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.ExcludeByFile="**/*Tests*/**/*.cs,**/*Tests*.cs" \
     DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.ExcludeByAttribute="ExcludeFromCodeCoverage,GeneratedCodeAttribute,CompilerGeneratedAttribute"

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
# method coverage: a method is covered when it contains at least one hit line
METHOD=$(python3 -c "
import xml.etree.ElementTree as ET
tree = ET.parse('$XML')
root = tree.getroot()
covered = 0; total = 0
for m in root.iter('method'):
    total += 1
    if any(int(l.attrib.get('hits','0')) > 0 for l in m.iter('line')):
        covered += 1
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
