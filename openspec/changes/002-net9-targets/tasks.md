# Tasks — 002-net9-targets

## Traceability matrix (capability → tasks)

| Capability | Covering tasks |
|---|---|
| Add net9.0 TFM | 1.1, 1.2, 1.3 |

## 1. Implementation

- [ ] 1.1 Add `net9.0` to `<TargetFrameworks>` in `src/Here.Sdk.Common/Here.Sdk.Common.csproj`.
  - **Acceptance:** `dotnet build` succeeds on all TFMs including `net9.0`.
  - **Verification:** `dotnet build -c Release` green; NuGet pack includes `lib/net9.0/` asset.

- [ ] 1.2 Add `net9.0` to the CI matrix in `.github/workflows/ci.yml`.
  - **Acceptance:** CI matrix includes `net9.0` in the build + test strategy.
  - **Verification:** CI workflow green on PR.

- [ ] 1.3 Update `global.json` to accept .NET 9 SDK alongside .NET 8 (rollForward policy).
  - **Acceptance:** `dotnet --version` resolves correctly under both SDK versions.
  - **Verification:** `dotnet build` green under .NET 9 SDK.

## Summary

- **Total tasks:** 3.
- **Blocked by:** 001-initial-release merged and tagged `v1.0.0`.
- **Blocks:** symmetric proposals in sibling repos.
