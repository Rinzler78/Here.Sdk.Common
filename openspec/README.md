# openspec/ — in 60 seconds

| Path | Purpose |
|---|---|
| `project.md` | Repo identity — stable. |
| `tech.md` | Tech stack + architectural decisions — stable. |
| `AGENTS.md` | Claude agent routing for this repo. |
| `specs/` | **Current** capabilities (empty until the first proposal lands). |
| `changes/<id>/` | **Proposed** changes in flight. |
| `changes/archive/<id>/` | Landed and archived proposals (history). |

## Reading order

1. `project.md` — what this repo is.
2. `tech.md` — how it is built.
3. `changes/001-initial-release/proposal.md` — the active proposal creating v1.0.
4. `changes/001-initial-release/specs/**/spec.md` — the delta specs (strict OpenSpec: `### Requirement: …` + `#### Scenario: …` + `- **WHEN** …` / `- **THEN** …`).
5. `changes/001-initial-release/tasks.md` — numbered tasks with Acceptance + Verification + Coverage.

## Proposing a new change

```bash
/open-change-proposal <slug>
```

Scaffolds `changes/<slug>/` with `proposal.md` + `tasks.md` + `design.md` + `specs/` placeholder. Fill them, open a PR. `spec-reviewer` validates automatically.

## Validating

```bash
./build/openspec-lint.sh
/validate-openspec
```
