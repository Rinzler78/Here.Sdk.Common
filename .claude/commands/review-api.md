---
description: Invoke package-api-reviewer on the current diff to check SemVer compliance and spec alignment.
allowed-tools: Bash(git diff:*), Bash(git log:*), Read, Agent(package-api-reviewer), mcp__serena__get_symbols_overview, mcp__serena__find_symbol
---

Run `package-api-reviewer` against the current branch diff.

Steps:
1. `git diff origin/develop...HEAD -- 'src/**/*.cs'`.
2. Load `openspec/changes/001-initial-release/specs/package-api/spec.md` (or current `openspec/specs/package-api/spec.md` once proposal landed).
3. Invoke `package-api-reviewer`.
4. Post the report inline.
