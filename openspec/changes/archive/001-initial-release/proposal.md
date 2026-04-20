# Change — Create Here.Sdk.Common v1.0 (initial release)

---
id: 001-initial-release
title: Create Here.Sdk.Common v1.0 (initial release)
status: archived
author: "@rinzler78"
reviewers: []
created: 2026-04-18
updated: 2026-04-19
target-specs:
  - openspec/specs/package-api/spec.md
  - openspec/specs/testing/spec.md
  - openspec/specs/coding-principles/spec.md
  - openspec/specs/performance/spec.md
  - openspec/specs/governance/spec.md
  - openspec/specs/openspec-methodology/spec.md
  - openspec/specs/github-provisioning/spec.md
semver-impact: MINOR
---

## Why

The `Here.Sdk.*` ecosystem is a community-maintained wrapper around the HERE SDK 4.x Navigate Edition for Xamarin.Forms 5 and .NET MAUI 8 consumers. The prior HERE Premium Mobile SDK v3 reached end-of-life on 2025-12-15, leaving no maintained .NET surface. Every downstream package in the ecosystem (`Abstractions`, `Core`, `Bindings.*`, `Android`, `iOS`, `Forms`) needs a shared vocabulary of primitive value objects, units, and errors; publishing those primitives in a dedicated zero-dependency package avoids duplication, locks invariants (WGS84 lat/lon bounds, bearing normalisation, distance units) in the type system, and keeps the other packages lean. This proposal authorises the creation of `Here.Sdk.Common` v1.0 and establishes the quality bar every subsequent proposal in this repo inherits.

## What changes

- **ADDED** `Here.Sdk.Common.Geography` — `GeoCoordinates`, `GeoBoundingBox`, `GeoPolyline`, `GeoBearing` as immutable records; `FlexiblePolyline` static encoder/decoder; `EarthConstants`.
- **ADDED** `Here.Sdk.Common.Units` — `Distance`, `Duration`, `Speed` records with idiomatic conversions and enums `DistanceUnit`, `SpeedUnit`.
- **ADDED** `Here.Sdk.Common.Errors` — `HereErrorCode` enum, `HereException` base + specialised subtypes (`HereNetworkException`, `HereAuthenticationException`, `HereRateLimitedException`, `HereInvalidRequestException`).
- **ADDED** testing strategy (90 % global + per-file coverage; test pyramid; determinism rules).
- **ADDED** coding principles (Clean Code, Clean Architecture layer rules as they apply to `Common`, SOLID with concrete patterns, DDD-lite value objects).
- **ADDED** performance budget (binary size ≤ 100 KB, `GeoCoordinates.DistanceTo` ≤ 50 ns zero-alloc, `FlexiblePolyline.Decode(1000 pts)` ≤ 5 ms).
- **ADDED** governance rules (branch protection, review counts, OpenSpec proposal workflow, Conventional Commits, Release Please automation).
- **ADDED** OpenSpec methodology — the rules every future change proposal in this repo must satisfy.
- **ADDED** GitHub provisioning: public repository on `rinzler78`, package-scoped NuGet API key (`NUGET_API_KEY` secret), GitHub Rulesets on `master`/`develop`, `nuget-production` environment, idempotent `build/setup-github.sh`.
- **ADDED** §1.0 Git bootstrap task (`git init` before §1.3 `pre-commit install`) and expanded §9.6 `setup-github.sh` acceptance — the idempotent script now also ensures `master` is committed/pushed, creates `develop` from `master` + sets it as default branch, closing the sequencing gap that previously left Rulesets without branches to protect.

## Impact

### Affected capabilities (specs introduced)

- `openspec/specs/package-api/spec.md` — **ADDED** (whole package public API).
- `openspec/specs/testing/spec.md` — **ADDED**.
- `openspec/specs/coding-principles/spec.md` — **ADDED**.
- `openspec/specs/performance/spec.md` — **ADDED**.
- `openspec/specs/governance/spec.md` — **ADDED**.
- `openspec/specs/openspec-methodology/spec.md` — **ADDED**.

### Affected consumers

- **External:** new public package; no existing consumers affected.
- **Internal (downstream siblings):** `Here.Sdk.Abstractions` will depend on this package. Its own initial-release proposal will declare the dependency.

### SemVer implications

- **MINOR** — an initial `1.0.0` release from `0.x` alpha series is MINOR in spirit (no prior `1.0.0` to break). All requirements are pure ADDED. Release Please will cut `1.0.0` on the first `feat:` commit after stabilisation; prereleases ship as `0.x.y-alpha.N` from `develop` in the meantime.

### Affected repos in the ecosystem

- `Here.Sdk.Common` (this repo).
- Downstream consumers (`Abstractions`, `Core`, `Android`, `iOS`, `Forms`, `Bindings.Android`, `Bindings.iOS`) will add a `PackageReference` to this package in their own initial-release proposals.

## Alternatives considered

- **A — Merge Common into Abstractions.** Rejected: couples primitive types to the contract surface; every consumer that wants only primitives would pull the `System.Reactive` dependency `Abstractions` requires.
- **B — Use existing geography library (NetTopologySuite, Geo).** Rejected: those targets are GIS-heavy with heavy dependencies; `Common` needs ≤ 100 KB binary size and zero deps.
- **C — Embed WGS84 constants in each downstream package.** Rejected: duplication risks divergence; centralising them in `Common` with invariants at construction is the Clean Code value-object prescription.

**Chosen:** dedicated zero-dep `Here.Sdk.Common` package.

## Out of scope

- REST client — belongs to `Here.Sdk.Core`.
- Platform-specific types — belong to `Bindings.*` and platform wrappers.
- UI primitives — belong to `Here.Sdk.Forms`.
- `IObservable<T>` contracts — belong to `Here.Sdk.Abstractions`.
- Magnetic-declination helpers — deferred to a future proposal.
- Localisation of error messages — HERE SDK messages stay in English; consumers localise.

## Open questions

- Should `GeoPolyline` precompute segment bearings lazily or eagerly? Deferred — measured once benchmarks exist (tracked in `tasks.md` phase 5).
- Should `Distance` implement `IComparable<Distance>` explicitly or rely on the record's structural comparison? Deferred to implementation; captured in `design.md`.

## Related

- First ADR set authored by this proposal: ADR-0001 through ADR-0005 under `docs/architecture/decision-records/`.
- Sibling proposals to follow: `Here.Sdk.Abstractions/changes/001-initial-release/`, `Here.Sdk.Core/changes/001-initial-release/`, etc.
