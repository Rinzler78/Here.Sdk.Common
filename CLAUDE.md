# CLAUDE.md — Here.Sdk.Common

Repository: primitive value objects, enums, exceptions, and neutral helpers for the `Here.Sdk.*` ecosystem. Zero runtime dependencies. TFM: `netstandard2.0;netstandard2.1;net8.0`. License: MIT. Community project, not affiliated with HERE Technologies.

## Non-negotiable rules (auto-enforced)

- **Language:** English (en-US) only — identifiers, docs, comments, commits, PRs, issues. Enforced by `cspell` + `codespell` in pre-commit and CI. Non-ASCII Latin/Cyrillic/CJK forbidden in `src/**/*.cs`.
- **Branches:** never commit directly on `master`, `develop`, `release/*`. Enforced by `.githooks/pre-commit` (local) + `.githooks/pre-push` (local) + GitHub Rulesets (remote). Direct push to `develop`/`master` is blocked both locally and on GitHub.
- **Worktree mandatory:** every task MUST use a git worktree. Command: `git worktree add ../Here.Sdk.Common-<branch> -b <type>/<slug>`. Never `git checkout` on the main clone. Merge only via PR to `develop`. Squash merge enforced.
- **Commits:** Conventional Commits only. Breaking changes via `feat!:` / `fix!:` + `BREAKING CHANGE:` footer. Release Please owns tags, CHANGELOG, version bumps.
- **Secrets:** no HERE credentials, NuGet keys, GitHub tokens anywhere. `detect-secrets` + `detect-here-credentials.sh` pre-commit hooks.
- **Coverage (hard gate):** ≥ 90 % line + 90 % branch + 95 % method **globally AND per file**. 100 % on public API. One file below threshold fails the PR.
- **Clean Code / Clean Architecture / SOLID** per `openspec/changes/001-initial-release/specs/coding-principles/spec.md`. No singletons, no service locator, constructor injection only, inward-pointing dependencies.
- **Performance:** `async`/`await` + `CancellationToken` on all async public methods. No `.Result`, `.Wait()`, `.GetAwaiter().GetResult()`. Hot paths annotated `// PERF: hot path` with allocation budget + BenchmarkDotNet baseline.
- **Tests:** xUnit + FluentAssertions + Moq. Deterministic (no `DateTime.Now`, no `Thread.Sleep`, no unsalted `Random`). Hermetic unit tests. `[SkippableFact]` for credentialed integration tests.
- **Documentation XML:** `<summary>` mandatory on every public type/member; `CS1591` = error in Release.

## Mandatory pre-PR gate

Run `./build/verify.sh` (once implemented by the initial-release proposal). Composes: `dotnet format --verify-no-changes`, build Release, test, coverage gate, clean-architecture check, pre-commit, openspec-lint, detect-here-credentials.

## OpenSpec workflow

Non-trivial changes flow through `openspec/changes/<slug>/`:

1. `/open-change-proposal <slug>` scaffolds `proposal.md` + `tasks.md` + `design.md` + `specs/` delta placeholder.
2. Author fills them per `specs/openspec-methodology/spec.md` rules.
3. `spec-reviewer` agent runs automatically and enforces: frontmatter, required sections, task `Acceptance`/`Verification`/`Coverage`, delta format (ADDED/MODIFIED/REMOVED/RENAMED requirements with `#### Scenario:` + `- **WHEN** …` / `- **THEN** …`), SemVer match.
4. On merge, the current `openspec/specs/` is updated from the delta; the proposal is moved to `openspec/changes/archive/<slug>/`.

## Agents in this repo (`.claude/agents/`)

- `spec-author` — scaffolds and authors OpenSpec files.
- `spec-reviewer` — structural + semantic + SemVer validation of OpenSpec artifacts.
- `package-api-reviewer` — public-surface watchdog; SemVer bump recommendations.
- `release-pr-reviewer` — reviews Release Please PRs.

Global system agents (`~/.claude/AGENTS.md`) remain available for code review, testing, security, docs, governance.

## MCP servers

`serena` (semantic C# navigation), `context7` (external docs), `filesystem`, `byterover-mcp`. Serena memory tools disabled (global policy).

## Fast-start commands

`/new-spec` · `/review-api` · `/bump-version` · `/open-change-proposal` · `/validate-openspec` · `/new-adr`

## Current state

This repository was just scaffolded. The **only active artifact** is `openspec/changes/001-initial-release/` — the proposal that drives the initial v1.0 realization. Read it first:

- [`openspec/changes/001-initial-release/proposal.md`](./openspec/changes/001-initial-release/proposal.md)
- [`openspec/changes/001-initial-release/tasks.md`](./openspec/changes/001-initial-release/tasks.md)
- [`openspec/changes/001-initial-release/design.md`](./openspec/changes/001-initial-release/design.md)
- [`openspec/changes/001-initial-release/specs/`](./openspec/changes/001-initial-release/specs/)

## References

- [`openspec/project.md`](./openspec/project.md)
- [`openspec/AGENTS.md`](./openspec/AGENTS.md)
- [`openspec/README.md`](./openspec/README.md)
