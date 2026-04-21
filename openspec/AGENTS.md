# Claude Agent Routing — Here.Sdk.Common

Inherits from `~/.ai-governance/AGENTS.md` → `~/.claude/AGENTS.md` → this file.

## Auto-routing triggers

| Trigger | Agent |
|---|---|
| Edit to `openspec/specs/**/*.md` or `openspec/changes/**/*.md` | `spec-reviewer` |
| Edit to `src/**/*.cs` touching a public type/member | `package-api-reviewer` |
| Edit to `src/**/*.cs` internal | `code-reviewer` (global) |
| Edit to `tests/**/*.cs` | `test-strategist` (global) |
| Edit to `*.csproj` / `Directory.*.props` | `architecture-strategist` (compound-engineering) |
| Hot-path annotated `// PERF: hot path` touched | `performance-oracle` (compound-engineering) |
| Credential / env-var patterns touched | `security-auditor` (global) + `security-sentinel` (compound-engineering) |
| PR authored by `release-please[bot]` or labeled `release-please` | `release-pr-reviewer` |

## Mandatory gate sequence before merge

1. `code-reviewer` — Clean Code / SOLID / Clean Arch.
2. `test-strategist` — tests present, F.I.R.S.T., 90 % per-file coverage.
3. `package-api-reviewer` — public surface and SemVer.
4. `security-auditor` — secret scan, deps vulns.
5. `performance-oracle` — benchmark regression (if hot path touched).
6. `spec-reviewer` — OpenSpec structure + consistency + SemVer alignment.
7. `release-pr-reviewer` — Release Please integrity (when applicable).

## Project agents (`.claude/agents/`)

- `spec-author.md`
- `spec-reviewer.md`
- `package-api-reviewer.md`
- `release-pr-reviewer.md`

## Checks owned

- `openspec_structure_lint`
- `openspec_cross_reference_integrity`
- `openspec_package_api_alignment`
- `openspec_proposal_structure_valid`
- `openspec_tasks_acceptance_criteria_present`
- `openspec_scenarios_well_formed`
- `openspec_semver_alignment`
- `package_api_semver_compliance`
- `coverage_global_90pct`
- `coverage_per_file_90pct`
- `clean_architecture_inward_rule`

## When not to delegate

Typo fixes, trivial formatting, single-line obvious edits, simple file reads.
