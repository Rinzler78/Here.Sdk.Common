---
id: 002-net9-targets
title: Add net9.0 TFM and expand target coverage
status: draft
author: "@rinzler78"
reviewers: []
created: 2026-04-19
updated: 2026-04-19
target-specs: []
semver-impact: MINOR
---

## Why

.NET 9.0 reached GA in November 2024. `Here.Sdk.Premium.Common` currently ships only `netstandard2.0;netstandard2.1;net8.0`. Consumers running .NET 9.0 can still consume the `net8.0` asset via forwards-compatibility, but they lose .NET 9 intrinsics, performance improvements, and in-box API additions. Adding `net9.0` as an explicit TFM surface closes that gap with no breaking change (MINOR — additive only).

## What changes

- **MODIFIED** `<TargetFrameworks>` in `Here.Sdk.Premium.Common.csproj`: add `net9.0`.
- **MODIFIED** CI matrix in `.github/workflows/ci.yml`: add `net9.0` to the build + test matrix.
- **MODIFIED** `global.json` or `Directory.Build.props` to support .NET 9 SDK alongside .NET 8.

## Impact

### Affected capabilities

No existing public API changes. New TFM asset added.

### Affected consumers

.NET 9 consumers receive a native `net9.0` binary instead of falling back to `net8.0`.

### SemVer implications

MINOR — additive TFM. No public API removed.

### Affected repos in the ecosystem

`Here.Sdk.Premium.Common` only. Downstream repos (Abstractions, Core, etc.) each carry their own symmetric proposal.

## Alternatives considered

- **Do nothing / rely on forwards-compat.** Rejected: misses net9.0 JIT and runtime improvements; signals stagnation.

## Out of scope

- net10.0 preview TFM.
- Windows / MacCatalyst targets (neutral layer — not applicable).

## Related

- `Here.Sdk.Premium.Abstractions/002-net9-targets` — symmetric.
- `Here.Sdk.Premium.Core/002-net9-targets` — symmetric.
