# Changelog

## [1.1.0](https://github.com/Rinzler78/Here.Sdk.Common/compare/v1.0.0...v1.1.0) (2026-05-03)

### Features

* add net9.0 TFM and expand target coverage ([1b4fcbd](https://github.com/Rinzler78/Here.Sdk.Common/commit/1b4fcbd))
* add net10.0 TFM + vulnerability and TFM checks in CI and hooks ([#31](https://github.com/Rinzler78/Here.Sdk.Common/pull/31)) ([2b999a7](https://github.com/Rinzler78/Here.Sdk.Common/commit/2b999a7))

### Bug Fixes

* address Copilot review -- hook exit codes, TFM grep scope, globstar ([#33](https://github.com/Rinzler78/Here.Sdk.Common/pull/33)) ([ae35693](https://github.com/Rinzler78/Here.Sdk.Common/commit/ae35693))
* prefix unused pre-push vars with _ to satisfy shellcheck SC2034 ([#38](https://github.com/Rinzler78/Here.Sdk.Common/pull/38)) ([207d68e](https://github.com/Rinzler78/Here.Sdk.Common/commit/207d68e))
* remove stale nuget-publish.yml resurrected from master merge ([#40](https://github.com/Rinzler78/Here.Sdk.Common/pull/40)) ([4004949](https://github.com/Rinzler78/Here.Sdk.Common/commit/4004949))
* full coverage -- outdated, readme, cspell locale, native fallback ([#42](https://github.com/Rinzler78/Here.Sdk.Common/pull/42)) ([5af82c4](https://github.com/Rinzler78/Here.Sdk.Common/commit/5af82c4))

### CI/CD

* integrate NuGet publish into CI pipeline, triggered on tag push ([#30](https://github.com/Rinzler78/Here.Sdk.Common/pull/30)) ([74dc895](https://github.com/Rinzler78/Here.Sdk.Common/commit/74dc895))
* publish on tag push `v*.*.*` instead of master branch push ([fix(ci): remove stale nuget-publish.yml resurrected from master merge (#40)](https://github.com/Rinzler78/Here.Sdk.Common/pull/40))

### Governance / Hooks

* add `.githooks/`, pre-push stage hooks, and extended linting ([#29](https://github.com/Rinzler78/Here.Sdk.Common/pull/29)) ([62a4014](https://github.com/Rinzler78/Here.Sdk.Common/commit/62a4014))
* hooks cover cspell (en-US), dotnet security, packages outdated, coverage ≥90%, readme, branch guard, shellcheck, format, documentation

---

## 1.0.0 (2026-04-22)

### Fixes

* v1.0.0 release — version.json fix and coverage gate ([#17](https://github.com/Rinzler78/Here.Sdk.Common/issues/17)) ([61f33cf](https://github.com/Rinzler78/Here.Sdk.Common/commit/61f33cfd7da13963b1c6b723ce574fea41d351f9))
