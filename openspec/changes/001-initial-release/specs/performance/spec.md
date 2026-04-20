# Delta — performance (ADDED)

## ADDED Requirements

### Requirement: Binary size budget ≤ 100 KB for the compressed `.nupkg`
The system SHALL keep the Release `Here.Sdk.Common.<version>.nupkg` under 100 KB. CI SHALL measure and fail if exceeded by more than 5 %.

#### Scenario: Package exceeds budget
- **WHEN** a PR produces a `.nupkg` larger than 105 KB
- **THEN** `build/measure-binary-size.sh` exits non-zero AND the PR is blocked

#### Scenario: Package within budget
- **WHEN** the `.nupkg` is ≤ 100 KB
- **THEN** the step passes


### Requirement: Hot-path allocation budgets declared and enforced
The system SHALL annotate every hot path with `// PERF: hot path` in code and document the allocation budget + target mean time in the XML doc `<remarks>`. A BenchmarkDotNet baseline SHALL exist under `tests/Here.Sdk.Common.Benchmarks/baselines/<method>.baseline.json`.

#### Scenario: GeoCoordinates.DistanceTo meets budget
- **WHEN** `GeoCoordinatesBenchmarks.DistanceTo_Haversine` runs on a modern machine
- **THEN** mean time is ≤ 50 ns AND allocation is 0 bytes

#### Scenario: GeoBearing constructor meets budget
- **WHEN** `GeoBearingBenchmarks.Constructor_Wrap` runs
- **THEN** mean time is ≤ 5 ns AND allocation is 0 bytes

#### Scenario: FlexiblePolyline.Decode on 1000 points meets budget
- **WHEN** `FlexiblePolylineBenchmarks.Decode_1000Points` runs
- **THEN** mean time is ≤ 5 ms AND allocation count is proportional to output array size only


### Requirement: Regression gate on benchmarks
The system SHALL fail CI when a tracked benchmark regresses by more than 10 % in mean time OR more than 5 % in allocations compared to the checked-in baseline.

#### Scenario: 15 % mean-time regression
- **WHEN** a PR changes `GeoCoordinates.DistanceTo` such that `DistanceTo_Haversine` mean time is 15 % higher than baseline
- **THEN** `build/bench.sh` exits non-zero AND CI fails

#### Scenario: Baseline refresh documented
- **WHEN** a maintainer runs `build/bench.sh --update-baseline`
- **THEN** the change to `baselines/*.json` is part of a PR whose body justifies the refresh


### Requirement: AOT / Trim friendliness
The system SHALL mark every production assembly `<IsTrimmable>true</IsTrimmable>` and SHALL forbid `Type.GetType(string)`, `Assembly.Load`, `MakeGenericType`, and similar dynamic reflection on the public surface.

#### Scenario: Trim-warning-free publish
- **WHEN** `dotnet publish -c Release -f net8.0 /p:PublishTrimmed=true` is run
- **THEN** the build completes with zero IL trim warnings

#### Scenario: Reflection API usage flagged
- **WHEN** a PR adds `Type.GetType("Foo.Bar")` in `src/`
- **THEN** analyzer `IL2026` fires AND build fails in Release


### Requirement: Deterministic and reproducible builds
The system SHALL produce bit-identical `.nupkg` bytes for identical source and toolchain by setting `<ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>`, `<Deterministic>true</Deterministic>`, and `<EmbedUntrackedSources>true</EmbedUntrackedSources>` in `Directory.Build.props`.

#### Scenario: Two CI runs on the same commit produce identical hashes
- **WHEN** two CI runs produce `.nupkg` for the same commit SHA
- **THEN** their SHA-256 digests are identical
