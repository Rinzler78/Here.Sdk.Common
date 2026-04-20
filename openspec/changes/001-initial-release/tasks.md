# Tasks — 001-initial-release

## Traceability matrix (capability → tasks)

Each capability in `specs/` must be covered by at least one task. This matrix is the single source of truth for spec-to-task traceability; every `### Requirement:` in a delta spec must map to a task below.

| Capability (spec folder) | Covering tasks |
|---|---|
| `package-api/spec.md` | 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 3.10, 3.11, 3.12, 3.13, 3.14, 3.15, 3.16, 3.17, 3.18, 4.1 |
| `testing/spec.md` | 1.3, 4.1, 4.2, 4.3, 4.4, 4.5, 8.2 |
| `coding-principles/spec.md` | 1.1 (analyzers, cspell, codespell), 1.3 (pre-commit), 3.1–3.5 (every src file respects the principles), 7.2 (ADRs), 8.1 (Conventional Commits) |
| `performance/spec.md` | 5.1, 5.2, 5.3 |
| `governance/spec.md` | 1.1 (setup-github.sh ruleset), 1.3 (pre-commit), 6.1, 6.2 (CI workflows), 8.1, 8.2 (Release Please), 2.1, 2.2 (archive rule) |
| `openspec-methodology/spec.md` | 1.1 (openspec-lint.sh), 2.1, 2.2 (archive + lint), 4.4 (openspec-lint script), 6.4 (openspec-validate.yml) |
| `github-provisioning/spec.md` | 9.1, 9.2, 9.3, 9.4, 9.5 (NuGet key scoped, GitHub repo + Rulesets + secrets + environment, idempotent setup-github.sh, credentials doc) |

Every task also references its `Acceptance`, `Verification`, and (where applicable) `Coverage`.

## 0. Execution order note

Although GitHub-provisioning tasks are grouped later for traceability, the intended execution order is:

1. complete the local bootstrap in §1 (`git init`, skeleton, local tooling);
2. run §9.2 + §9.6 immediately after bootstrap so the GitHub repository, `master`, and `develop` exist before the bulk of the implementation starts;
3. finish the remaining provisioning tasks in §9.1, 9.3–9.5, 9.7;
4. continue with specs, implementation, tests, CI, docs, and release tasks.

## 1. Repository bootstrap

- [ ] 1.0 Initialise the local Git repository (`git init`) before §1.1 scaffolding is committed and before §1.3 `pre-commit install` — pre-commit hooks require `.git` to operate.
  - **Acceptance:** `git init` executed at repo root with `git config init.defaultBranch master`; `.git/` present; bootstrap exception documented — the branch-guard hook (forbid direct commit on `master`/`develop`) applies only after §9.6 `setup-github.sh` has pushed `master` + `develop` and applied Rulesets.
  - **Verification:** `git rev-parse --is-inside-work-tree` returns `true`; `ls .git/HEAD` exists.

- [ ] 1.1 Initialise the repo skeleton (`src/`, `tests/`, `samples/`, `build/`, `docs/`, `.github/`, `global.json`, `Directory.Build.props`, `Directory.Packages.props`, `NuGet.config`, `version.json`, `.editorconfig`, `.gitignore`, `.gitattributes`, `.markdownlint.json`, `.yamllint`, `.pre-commit-config.yaml`, `.cspell.json`, `.cspell/here-ecosystem.txt`, `.codespellignore`).
  - **Acceptance:** all files present; `dotnet new gitignore` not used (hand-curated per spec); `global.json` pins SDK `8.0.400`.
  - **Verification:** `./build/verify.sh --skip-test` green on the empty repo.

- [ ] 1.2 Install `Nerdbank.GitVersioning` via `.config/dotnet-tools.json` and write `version.json` seeded at `"version": "0.1"`.
  - **Acceptance:** `nbgv get-version -v NuGetPackageVersion` returns a non-empty value.
  - **Verification:** command runs clean on `develop`.

- [ ] 1.3 Install `pre-commit` and wire every hook listed in the hooks section below.
  - **Acceptance:** `pre-commit install` + `--hook-type commit-msg` + `--hook-type pre-push` succeed.
  - **Verification:** `pre-commit run --all-files` green on the empty repo.

## 2. Shared specs land (this proposal's deltas become current specs)

- [ ] 2.1 On merge of this PR, move every file from `openspec/changes/001-initial-release/specs/` to `openspec/specs/` (1:1 path mapping).
  - **Acceptance:** `openspec/specs/package-api/spec.md`, `openspec/specs/testing/spec.md`, `openspec/specs/coding-principles/spec.md`, `openspec/specs/performance/spec.md`, `openspec/specs/governance/spec.md`, `openspec/specs/openspec-methodology/spec.md` exist with the same content, updated frontmatter `status: accepted`.
  - **Verification:** `./build/openspec-lint.sh` green.

- [ ] 2.2 Archive this proposal under `openspec/changes/archive/001-initial-release/` with frontmatter `status: archived`.
  - **Acceptance:** directory moved; git history preserved (no squash of the move).
  - **Verification:** `git log --follow` resolves both old and new paths.

## 3. Public API implementation

- [ ] 3.1 Implement `GeoCoordinates` under `src/Geography/GeoCoordinates.cs`.
  - **Acceptance:** `public readonly record struct GeoCoordinates(double Latitude, double Longitude, double? Altitude = null)`; primary ctor throws `ArgumentOutOfRangeException` when `Latitude ∉ [-90, 90]` or `Longitude ∉ [-180, 180]`; `ToString()` renders invariant-culture string.
  - **Verification:** `tests/…/Geography/GeoCoordinatesTests.cs` covers valid, out-of-range, altitude-null, equality, ToString with `fr-FR` current culture.
  - **Coverage:** 100 % line + branch on `GeoCoordinates.cs`.

- [ ] 3.2 Implement `GeoBearing` under `src/Geography/GeoBearing.cs`.
  - **Acceptance:** `public readonly record struct GeoBearing(double DegreesFromNorth)`; normalises input to `[0, 360)` in primary ctor.
  - **Verification:** tests cover valid, negative wrap, overflow wrap, equality, GetHashCode across normalised values.
  - **Coverage:** 100 % line + branch.

- [ ] 3.3 Implement `GeoBoundingBox`, `GeoPolyline`, `EarthConstants`, `FlexiblePolyline` under `src/Geography/`.
  - **Acceptance:** each type matches its `Requirement` in `specs/package-api/spec.md`; `FlexiblePolyline.Decode` round-trips with `Encode`.
  - **Verification:** per-type test class with one `[Fact]` per documented scenario.
  - **Coverage:** 100 % line + branch on each file.

- [ ] 3.4 Implement `Distance`, `Duration`, `Speed`, `DistanceUnit`, `SpeedUnit` under `src/Units/`.
  - **Acceptance:** conversions symmetric; arithmetic operators (`+`, `-`, `*` by scalar) defined; `Distance.Zero` constant.
  - **Verification:** parameterised `[Theory]` tests cover all unit conversions; property-based tests via FsCheck for symmetry (optional — tracked as WARN).
  - **Coverage:** 100 % line + branch.

- [ ] 3.5 Implement `HereErrorCode`, `HereException` + subclasses under `src/Errors/`.
  - **Acceptance:** exception hierarchy rooted at `HereException : Exception`; each subclass carries the expected auxiliary data (`RetryAfter`, `FieldName`).
  - **Verification:** tests cover construction, serialisation round-trip (`.NET 8` `ISerializable` where applicable), `Code` property set.
  - **Coverage:** 100 % line + branch.

- [ ] 3.6 Implement extended geography types: `GeoCircle`, `GeoPolygon`, `GeoCorridor`, `GeoOrientation` under `src/Geography/`.
  - **Acceptance:** `GeoCircle` validates `RadiusInMeters >= 0`; `GeoPolygon` validates ≥ 3 vertices; `GeoCorridor` validates non-negative half-width; `GeoOrientation` normalises bearing to `[0, 360)` and clamps tilt/roll.
  - **Verification:** per-type test class with one `[Fact]` per documented BDD scenario in `specs/package-api/spec.md`.
  - **Coverage:** 100 % line + branch on each file.

- [ ] 3.7 Implement 2D/3D geometry value types under `src/Geometry/`: `Point2D`, `Point3D`, `Anchor2D`, `Size2D`, `Rectangle2D`, `Angle`, `AngleRange`, `IntegerRange`.
  - **Acceptance:** all are `readonly record struct`; `Angle.FromRadians` / `ToRadians` round-trips within 1e-9; `IntegerRange` rejects `Min > Max`; `Size2D` rejects negative dimensions.
  - **Verification:** parameterised `[Theory]` covering valid, boundary, and error cases.
  - **Coverage:** 100 % line + branch.

- [ ] 3.8 Implement `Location` record under `src/Positioning/Location.cs`.
  - **Acceptance:** required fields `Coordinates` + `Timestamp`; all optional sensor fields nullable; value equality by all fields.
  - **Verification:** tests cover minimal construction, full construction, equality, `GetHashCode` consistency.
  - **Coverage:** 100 % line + branch.

- [ ] 3.9 Implement localisation types under `src/Localisation/`: `LanguageCode`, `CountryCode`, `LocalizedText`, `LocalizedTexts`.
  - **Acceptance:** `LanguageCode` and `CountryCode` validate non-empty input; `LocalizedTexts.GetText` returns preferred language or falls back to first entry.
  - **Verification:** tests cover: preferred language returned, fallback to first when preferred absent, empty list handled gracefully (returns null).
  - **Coverage:** 100 % line + branch.

- [ ] 3.10 Implement identifier types under `src/Identifiers/`: `NameId`, `ExternalId`.
  - **Acceptance:** both validate non-empty `Name`/`Id`/`Provider` fields; value equality by all fields.
  - **Verification:** tests cover construction, equality, and ArgumentException on empty input.
  - **Coverage:** 100 % line + branch.

- [ ] 3.11 Implement `UnitSystem`, `CardinalDirection` enums under `src/Units/` and `src/Geography/` respectively.
  - **Acceptance:** `UnitSystem.Metric` is ordinal 0; `CardinalDirection.North` is ordinal 0; no values inserted between existing members.
  - **Verification:** `(int)UnitSystem.Metric == 0` and `(int)CardinalDirection.North == 0` asserted.
  - **Coverage:** enum files excluded from per-file coverage gate (pure data, no logic); enum-constant ordinal tests in `3.11` task test file.

- [ ] 3.12 Implement transport-domain enums under `src/Transport/`: `TransportMode`, `VehicleType`, `TruckType`, `TruckCategory`, `TruckClass`, `FuelType`, `TruckFuelType`, `HazardousMaterial`, `TunnelCategory`.
  - **Acceptance:** `HazardousMaterial` is a `[Flags]` enum; all compound flag combinations are expressible; ordinals stable (append-only).
  - **Verification:** flags test: `Explosive | Gas` sets both bits; test for every enum's first-member ordinal = 0.
  - **Coverage:** enum files excluded from per-file coverage gate (see §3.11 note).

- [ ] 3.13 Implement routing-domain enums under `src/Routing/`: `WaypointType`, `OptimizationMode`, `ManeuverAction`, `SectionTransportMode`, `NoticeSeverity`, `IsolineRangeType`, `PaymentMethod`, `ChargingConnectorType`, `ChargingSupplyType`, `RoutePlaceType`.
  - **Acceptance:** `ChargingConnectorType.SaeJ3400` present (NACS support); append-only contract documented via `[EditorBrowsable(EditorBrowsableState.Never)]` test comment.
  - **Verification:** all enum members compilable; `SaeJ3400` present in the enum definition.
  - **Coverage:** enum files excluded from per-file gate.

- [ ] 3.14 Implement search-domain enums under `src/Search/`: `PlaceType`, `AddressType`, `DayOfWeek`, `EvseStatus`, `EvAccessType`.
  - **Acceptance:** `DayOfWeek.Monday` ordinal = 0; `EvseStatus.Unknown` is a valid discriminator value.
  - **Verification:** ordinal stability test for `DayOfWeek.Monday`.
  - **Coverage:** enum files excluded from per-file gate.

- [ ] 3.15 Implement navigation-domain enums under `src/Navigation/`: `MilestoneType`, `MilestoneStatus`, `SpeedWarningStatus`, `RoadClassification`, `LaneRecommendationState`, `BorderCrossingType`, `SafetyCameraType`, `TollCollectionMethod`.
  - **Acceptance:** `SpeedWarningStatus` has exactly 2 members (`SpeedLimitExceeded`, `SpeedLimitRestored`); switch expression covering both compiles without `CS8509`.
  - **Verification:** compile-time test (source generator or analyzer test) confirming exhaustiveness.
  - **Coverage:** enum files excluded from per-file gate.

- [ ] 3.16 Implement traffic-domain enums under `src/Traffic/`: `TrafficIncidentType`, `TrafficIncidentImpact`.
  - **Acceptance:** `TrafficIncidentImpact` members ordered so `Unknown < LowImpact < Minor < Major < Critical`.
  - **Verification:** ordinal comparison test.
  - **Coverage:** enum files excluded from per-file gate.

- [ ] 3.17 Implement positioning-domain enums under `src/Positioning/`: `LocationAccuracy`.
  - **Acceptance:** `LocationAccuracy.NavigationAccuracy` is ordinal 0 (safe default).
  - **Verification:** `default(LocationAccuracy) == LocationAccuracy.NavigationAccuracy` asserted.
  - **Coverage:** enum file excluded from per-file gate.

- [ ] 3.18 Implement EV and map domain enums: `EvChargingConnectorFormat`, `EvseState` under `src/Ev/`; `MapScheme`, `LineCap`, `VisibilityState`, `MapProjection` under `src/Map/`.
  - **Acceptance:** `EvseState.Unknown` is NOT ordinal 0 (avoid false-positive default); `MapScheme.NormalDay` is ordinal 0.
  - **Verification:** `default(EvseState) != EvseState.Available` (safety check); `default(MapScheme) == MapScheme.NormalDay`.
  - **Coverage:** enum files excluded from per-file gate.

## 4. Tests and quality gates

- [ ] 4.1 Author `tests/Here.Sdk.Premium.Common.UnitTests/` with xUnit + FluentAssertions + coverlet.collector.
  - **Acceptance:** project builds under all TFMs; `dotnet test` discovers and runs ≥ 1 test.
  - **Verification:** CI `build-netstandard` job green.
  - **Coverage:** see individual tasks; aggregate repo coverage ≥ 90 % line + 90 % branch + 95 % method.

- [ ] 4.2 Implement `build/coverage-gate.sh` and `.ps1` that parses `artifacts/coverage/coverage.cobertura.xml` and fails on any file below 90 % line or 90 % branch or 95 % method.
  - **Acceptance:** script exits non-zero when any `src/**/*.cs` file is below threshold; exits 0 when all pass.
  - **Verification:** seeded test with one intentionally-under-covered file triggers exit 1; removing it yields exit 0.

- [ ] 4.3 Implement `build/clean-architecture-check.sh` and `.ps1` that parses csproj graph and ensures no upward dependency (per the layer rules in `coding-principles/spec.md`).
  - **Acceptance:** reports zero violations for `Common`; a seeded `Common → Abstractions` reference triggers exit 1.
  - **Verification:** dry-run against the repo.

- [ ] 4.4 Implement `build/openspec-lint.sh` and `.ps1` enforcing the validation rules from `openspec-methodology/spec.md`.
  - **Acceptance:** BLOCK on missing frontmatter, missing sections, scenario without WHEN/THEN, SemVer mismatch; exit 0 when clean.
  - **Verification:** run against this proposal pre-merge → green.

- [ ] 4.5 Implement `build/detect-here-credentials.sh` and `.ps1` scanning for HERE credential patterns.
  - **Acceptance:** flags a seeded fake credential in a staged file; exits 0 on the clean repo.
  - **Verification:** unit test with seeded fake.

## 5. Performance

- [ ] 5.1 Author `tests/Here.Sdk.Premium.Common.Benchmarks/` with BenchmarkDotNet.
  - **Acceptance:** project builds; `[MemoryDiagnoser]` attributes set; baselines folder exists.
  - **Verification:** `dotnet run -c Release --project tests/…Benchmarks/` completes.

- [ ] 5.2 Establish baselines for the hot-path methods annotated `// PERF: hot path`: `GeoCoordinates.DistanceTo`, `GeoBearing` constructor, `FlexiblePolyline.Decode`.
  - **Acceptance:** `baselines/*.json` files committed; each documents the allocation budget and the target mean time.
  - **Verification:** `./build/bench.sh` green; regression > 10 % mean time or > 5 % allocations fails.
  - **Coverage:** not applicable — benchmark code excluded from coverage via `[ExcludeFromCodeCoverage]`.

- [ ] 5.3 Implement `build/measure-binary-size.sh` and `.ps1` that reports the Release `.nupkg` size and fails if > 100 KB.
  - **Acceptance:** script outputs size in bytes + JSON report under `artifacts/size-report.json`.
  - **Verification:** current pack is well under budget.

## 6. CI / CD

- [ ] 6.1 Author `.github/workflows/ci.yml` — matrix `ubuntu-latest` × `Debug|Release`, steps `setup-dev-env`, `verify`, `build`, `test`, `coverage-gate`, `pack`.
  - **Acceptance:** workflow green on PR.
  - **Verification:** open a test PR; CI green.

- [ ] 6.2 Author `.github/workflows/release-please.yml` with `googleapis/release-please-action@v4` and `release-please-config.json` / `release-please-manifest.json`.
  - **Acceptance:** push to `master` creates or updates a release PR.
  - **Verification:** merge a `feat:` commit and observe Release Please behaviour.

- [ ] 6.3 Author `.github/workflows/nuget-publish.yml` triggered by `release.published`.
  - **Acceptance:** workflow pushes the `.nupkg` + `.snupkg` to nuget.org with `NUGET_API_KEY`.
  - **Verification:** dry run on a test tag.

- [ ] 6.4 Author `.github/workflows/openspec-validate.yml` triggered on PRs touching `openspec/**`.
  - **Acceptance:** workflow runs `./build/openspec-lint.sh` and fails on BLOCK.
  - **Verification:** seeded bad proposal fails; clean proposal passes.

## 7. Documentation

- [ ] 7.1 Author `README.md` with the 13 mandatory sections per `openspec/specs/readme/spec.md` (to be added later; for now inline the list in `README.md`).
  - **Acceptance:** sections present: title, badges, install, quickstart, features, platforms, credentials link, docs links, sample, contributing, security, license, HERE disclaimer.
  - **Verification:** `markdownlint` green; reviewer confirms rendering on GitHub.

- [ ] 7.2 Author `CONTRIBUTING.md`, `CODE_OF_CONDUCT.md` (Contributor Covenant 2.1), `SECURITY.md`, `LICENSE` (MIT).
  - **Acceptance:** all four files present with valid content.
  - **Verification:** `pre-commit run --all-files` green.

- [ ] 7.3 Author ADR-0001 through ADR-0005 under `docs/architecture/decision-records/` per the list in `tech.md`.
  - **Acceptance:** 5 ADR files with sequential IDs, `status: accepted`, structured per ADR template.
  - **Verification:** `openspec-lint` integrity check resolves cross-references from `tech.md`.

- [ ] 7.4 Author `docs/getting-started.md` and `docs/credentials-setup.md` stubs.
  - **Acceptance:** minimal but accurate content; links from README resolve.
  - **Verification:** reviewer pass.

## 9. GitHub provisioning (public repo + NuGet key)

- [ ] 9.1 Generate a NuGet.org API key **scoped to `Here.Sdk.Premium.Common` only**, action `Push new packages and package versions`, expiry 365 days.
  - **Acceptance:** on `https://www.nuget.org/account/apikeys`, the key's `Selected Packages` list contains exactly `Here.Sdk.Premium.Common`.
  - **Verification:** paste the full key into `gh secret set NUGET_API_KEY -b - -R rinzler78/Here.Sdk.Premium.Common`; `gh secret list -R rinzler78/Here.Sdk.Premium.Common` shows `NUGET_API_KEY`.

- [ ] 9.2 Create the public GitHub repository under `rinzler78`.
  - **Acceptance:** `gh repo create rinzler78/Here.Sdk.Premium.Common --public --description "<from project.md>" --homepage "https://www.nuget.org/packages/Here.Sdk.Premium.Common" --disable-wiki` succeeds; Discussions enabled via `gh api -X PATCH repos/rinzler78/Here.Sdk.Premium.Common -F has_discussions=true`.
  - **Verification:** `gh repo view rinzler78/Here.Sdk.Premium.Common --json visibility,hasWikiEnabled,hasDiscussionsEnabled` returns `PUBLIC`, `false`, `true`.

- [ ] 9.3 Configure repository topics.
  - **Acceptance:** topics set to `here`, `maps`, `nuget`, `dotnet`, `xamarin`, `maui`, `csharp` via `gh api -X PUT repos/rinzler78/Here.Sdk.Premium.Common/topics --input <(echo '{"names":["here","maps","nuget","dotnet","xamarin","maui","csharp"]}')`.
  - **Verification:** `gh repo view --json repositoryTopics` lists the 7 topics.

- [ ] 9.4 Apply branch-protection Rulesets on `master` and `develop` via `build/setup-github.sh`.
  - **Acceptance:** Rulesets defined in `specs/git-flow/spec.md` (merged from proposal) applied: PR required (1 for `develop`, 2 for `master`), required status checks, signed commits, linear history, block force push + deletion; on `master` only, restrict direct updates to `release-please[bot]`.
  - **Verification:** manual test push to `master` rejected with ruleset violation; `gh api repos/.../rulesets` returns the expected JSON.

- [ ] 9.5 Create `nuget-production` GitHub Environment with required reviewers and `master`-only deployment.
  - **Acceptance:** `gh api repos/rinzler78/Here.Sdk.Premium.Common/environments/nuget-production --input <(<json>)` returns 200; `protection_rules.required_reviewers` non-empty; `deployment_branch_policy.custom_branch_policies` restricted to `master`.
  - **Verification:** `nuget-publish.yml` references `environment: nuget-production` and requires approval before publishing.

- [ ] 9.6 Implement `build/setup-github.sh` + `.ps1` idempotent (covers both the Git bootstrap hand-off and the GitHub provisioning).
  - **Acceptance:** the script performs — idempotently and in this order — (a) ensure `git init` has run (preconditioned by §1.0, no-op otherwise), (b) ensure a `master` commit exists and `git push origin master`, (c) `gh repo create rinzler78/Here.Sdk.Premium.Common --public …` if the remote is absent, (d) create `develop` from `master`, push, and set as default via `gh repo edit --default-branch develop`, (e) creates/updates repo settings, Rulesets, secrets, environment, Dependabot; re-running reports `No changes applied` and exits 0.
  - **Verification:** run the script twice; diff of observed settings before vs. after second run = no diff; `gh api repos/rinzler78/Here.Sdk.Premium.Common --jq .default_branch` returns `develop`; `git ls-remote origin` lists both `refs/heads/master` and `refs/heads/develop`.

- [ ] 9.7 Author `docs/credentials-setup.md` covering `NUGET_API_KEY` (how to generate package-scoped, how to register via `gh secret set`, how to rotate every 365 days).
  - **Acceptance:** the doc includes exact `gh` and nuget.org commands + a rotation procedure with a reminder issue opened at day 330.
  - **Verification:** `markdownlint` + `cspell` green; reviewer follows the doc end-to-end on a fresh machine.

## 10. Release preparation

- [ ] 10.1 Use a single Conventional Commit when the implementation lands: `feat: initial Here.Sdk.Premium.Common v0.1.0-alpha.1 implementation`.
  - **Acceptance:** Release Please proposes a prerelease PR; once stable, `feat!:` commit promotes to `1.0.0`.
  - **Verification:** Release Please preview in PR description matches expected bump.

- [ ] 10.2 Once stable, merge the Release Please PR on `master` → tag `v1.0.0` → GitHub Release → `nuget-publish.yml` pushes to nuget.org.
  - **Acceptance:** package visible on nuget.org with version `1.0.0`, license `MIT`, repository URL correct, README rendered.
  - **Verification:** external consumer adds `<PackageReference Include="Here.Sdk.Premium.Common" Version="1.0.0" />` and uses `GeoCoordinates` successfully.

## Summary

- **Total tasks:** 49 (phases 1 – 10).
- **Est. effort:** ~5 days of focused work (bootstrap + public types including full HERE SDK vocabulary + tests + scripts + workflows + docs + GitHub provisioning + NuGet key).
- **Blocked by:** none.
- **Blocks:** downstream sibling `Here.Sdk.Premium.Abstractions/changes/001-initial-release/`.
