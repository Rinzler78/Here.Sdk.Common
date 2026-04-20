---
description: Create a new OpenSpec capability document under openspec/specs/<name>/ for a capability that has already been merged (post-proposal).
argument-hint: <spec-name>
allowed-tools: Read, Write, Bash(ls:*), Bash(./build/openspec-lint.sh), Agent(spec-author)
---

Create `openspec/specs/$1/spec.md` in strict OpenSpec format.

Steps:
1. Verify `openspec/specs/$1/` does not already exist.
2. Load `openspec/project.md` + `openspec/tech.md` for context.
3. Invoke `spec-author` agent to produce a file with `## ADDED Requirements` and at least one `### Requirement:` + `#### Scenario:` + `- **WHEN**` / `- **THEN**`.
4. Run `./build/openspec-lint.sh` (if present); surface failures.
5. Output the created file path.
