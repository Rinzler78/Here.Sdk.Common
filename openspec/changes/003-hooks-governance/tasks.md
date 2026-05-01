# Tasks — 003-hooks-governance

## Traceability matrix

| Change | Task |
|---|---|
| `.githooks/` creation | T1 |
| `core.hooksPath` config | T2 |
| `RestorePackagesWithLockFile` + lock files | T3 |
| `.pre-commit-config.yaml` update | T4 |
| `pre-commit install` stages | T5 |
| `CLAUDE.md` fix | T6 |
| `openspec/specs/governance/spec.md` update | T7 |

## Tasks

- [x] **T1** — Create `.githooks/pre-commit`, `.githooks/pre-push`, `.githooks/commit-msg`
  from Meta canonical scripts
- [x] **T2** — Verify `git config core.hooksPath = .githooks` (already set)
- [x] **T3** — Add `RestorePackagesWithLockFile=true` to `Directory.Build.props`; run
  `dotnet restore`; commit three `packages.lock.json` files
- [x] **T4** — Update `.pre-commit-config.yaml` from Meta canonical
- [ ] **T5** — Run `pre-commit install --hook-type pre-push && pre-commit install
  --hook-type commit-msg` after merge
- [x] **T6** — Fix `CLAUDE.md` line 8: `main` -> `master`
- [x] **T7** — Add six new requirements to `openspec/specs/governance/spec.md`
