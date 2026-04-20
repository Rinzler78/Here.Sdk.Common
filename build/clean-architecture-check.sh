#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
SRC="$ROOT/src/Here.Sdk.Premium.Common/Here.Sdk.Premium.Common.csproj"

if [ ! -f "$SRC" ]; then
  echo "ERROR: Main project not found at $SRC" >&2
  exit 1
fi

# Check that the main project has zero PackageReference items (no runtime deps)
REFS=$(python3 -c "
import xml.etree.ElementTree as ET
tree = ET.parse('$SRC')
ns = {'m': 'http://schemas.microsoft.com/developer/msbuild/2003'}
# Try with and without namespace
refs = [e.attrib.get('Include','') for e in tree.iter('PackageReference')
        if 'PrivateAssets' not in {c.tag.split('}')[-1] for c in e}
           and e.attrib.get('Include','') not in ('Nerdbank.GitVersioning',)]
print('\n'.join(refs))
" 2>/dev/null || grep -oP '(?<=Include=\")[^\"]+' "$SRC" | grep -v "Nerdbank" || true)

if [ -n "$REFS" ]; then
  echo "ERROR: Runtime PackageReference found in Common — violates zero-dependency rule:" >&2
  echo "$REFS" >&2
  exit 1
fi

# Check no reference to sibling Here.Sdk.Premium.* packages
if grep -r "Here\.Sdk\.Premium\." "$ROOT/src/" --include="*.csproj" | grep -v "Here\.Sdk\.Premium\.Common"; then
  echo "ERROR: Reference to sibling Here.Sdk.Premium.* package found — violates inward dependency rule" >&2
  exit 1
fi

echo "✓ Clean architecture check passed (zero runtime deps, no upward references)"
