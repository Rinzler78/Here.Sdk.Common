---
description: Scaffold a new OpenSpec change proposal under openspec/changes/<slug>/ with proposal.md + tasks.md + design.md + specs/ delta placeholder.
argument-hint: <slug> [<target-capability>]
allowed-tools: Read, Write, Bash(ls:*), Bash(mkdir:*), Bash(date:*), Bash(git config:*), Agent(spec-author)
---

Scaffold `openspec/changes/$1/` with the full OpenSpec structure.

Steps:
1. Validate `$1` (kebab-case, matches `^[a-z][a-z0-9-]*[a-z0-9]$`, ≤ 50 chars).
2. Default target capability to `package-api` if `$2` not provided.
3. Resolve metadata: CHANGE_ID=$1, TITLE (prompt user for a one-line imperative title), DATE=today, AUTHOR=@(git config user.name).
4. Create `openspec/changes/$1/` with:
   - `proposal.md` (Why, What changes, Impact, Alternatives, Out of scope, Open questions — YAML frontmatter).
   - `tasks.md` (at least 1 task with ID + checkbox + Acceptance + Verification — Coverage if src/ touched; Summary block).
   - `design.md` (stub — delete if low-risk).
   - `specs/<capability>/spec.md` (placeholder with `## ADDED Requirements` + one `### Requirement: TODO` + one `#### Scenario:` stub).
5. Remind the user to:
   - Fill Why / What / Impact in proposal.md.
   - Flesh out the delta spec.
   - Populate tasks with Acceptance + Verification + Coverage where relevant.
   - Run `/validate-openspec` before opening the PR.
6. Do NOT create a branch, commit, or push.
