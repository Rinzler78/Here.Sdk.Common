#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

BASELINES_DIR="tests/Here.Sdk.Common.Benchmarks/baselines"
REPORT="BenchmarkDotNet.Artifacts/results/Here.Sdk.Common.Benchmarks.GeographyBenchmarks-report-brief.json"
UPDATE_BASELINE=0

for arg in "$@"; do
  case "$arg" in
    --update-baseline) UPDATE_BASELINE=1 ;;
    *) echo "Unknown argument: $arg" >&2; exit 1 ;;
  esac
done

echo "=== Running benchmarks ==="
dotnet run -c Release --project tests/Here.Sdk.Common.Benchmarks/ -- --filter "*"

if [ ! -f "$REPORT" ]; then
  echo "ERROR: BDN report not found at $REPORT" >&2
  exit 1
fi

if [ "$UPDATE_BASELINE" -eq 1 ]; then
  echo "=== Updating baselines ==="
  python3 - "$REPORT" "$BASELINES_DIR" <<'PYEOF'
import json, sys, os, datetime

report_path, baselines_dir = sys.argv[1], sys.argv[2]
with open(report_path) as f:
    report = json.load(f)

os.makedirs(baselines_dir, exist_ok=True)
for b in report["Benchmarks"]:
    method = b["Method"]
    mean_ns = b["Statistics"]["Mean"]
    alloc = b["Memory"]["BytesAllocatedPerOperation"]
    env = report["HostEnvironmentInfo"]
    note = f"{env['ProcessorName']}, {env['RuntimeVersion']}, {env['OsVersion'][:30]}"
    baseline = {
        "Method": method,
        "MeanNanoseconds": round(mean_ns, 2),
        "AllocatedBytesPerOperation": alloc,
        "CapturedAt": datetime.date.today().isoformat(),
        "Note": note,
    }
    path = os.path.join(baselines_dir, f"{method}.baseline.json")
    with open(path, "w") as f:
        json.dump(baseline, f, indent=2)
        f.write("\n")
    print(f"  updated {path}")
print("Baselines updated — commit the changes.")
PYEOF
  exit 0
fi

echo "=== Comparing against baselines ==="
python3 - "$REPORT" "$BASELINES_DIR" <<'PYEOF'
import json, sys, os

MEAN_THRESHOLD = 0.10   # 10 % regression triggers failure
ALLOC_THRESHOLD = 0.05  # 5 % regression triggers failure

report_path, baselines_dir = sys.argv[1], sys.argv[2]
with open(report_path) as f:
    report = json.load(f)

fail = False
rows = []

for b in report["Benchmarks"]:
    method = b["Method"]
    new_mean = b["Statistics"]["Mean"]
    new_alloc = b["Memory"]["BytesAllocatedPerOperation"]

    baseline_path = os.path.join(baselines_dir, f"{method}.baseline.json")
    if not os.path.exists(baseline_path):
        print(f"  WARN  {method}: no baseline file at {baseline_path}")
        continue

    with open(baseline_path) as f:
        baseline = json.load(f)

    base_mean = baseline["MeanNanoseconds"]
    base_alloc = baseline["AllocatedBytesPerOperation"]

    mean_delta = (new_mean - base_mean) / base_mean if base_mean > 0 else 0.0
    alloc_ok = True
    alloc_msg = "ok"

    if base_alloc == 0 and new_alloc > 0:
        alloc_ok = False
        alloc_msg = f"NEW ALLOC {new_alloc}B"
    elif base_alloc > 0:
        alloc_delta = (new_alloc - base_alloc) / base_alloc
        if alloc_delta > ALLOC_THRESHOLD:
            alloc_ok = False
            alloc_msg = f"+{alloc_delta*100:.1f}% ({new_alloc}B vs {base_alloc}B)"
        else:
            alloc_msg = f"{alloc_delta*100:+.1f}% ({new_alloc}B)"

    mean_ok = mean_delta <= MEAN_THRESHOLD
    status = "PASS" if (mean_ok and alloc_ok) else "FAIL"
    if not (mean_ok and alloc_ok):
        fail = True

    rows.append((status, method, f"{mean_delta*100:+.1f}%", f"{new_mean:.1f}ns vs {base_mean:.1f}ns", alloc_msg))

col_w = max(len(r[1]) for r in rows) if rows else 30
print(f"  {'Status':<6}  {'Method':<{col_w}}  {'Mean Δ':<8}  {'Mean':<28}  Alloc")
print("  " + "-" * (6 + 2 + col_w + 2 + 8 + 2 + 28 + 2 + 20))
for status, method, mean_delta, mean_info, alloc_msg in rows:
    print(f"  {status:<6}  {method:<{col_w}}  {mean_delta:<8}  {mean_info:<28}  {alloc_msg}")

if fail:
    print("\nBenchmark regression detected — run './build/bench.sh --update-baseline' if intentional.", file=sys.stderr)
    sys.exit(1)
else:
    print("\n✓ No regression detected")
PYEOF
