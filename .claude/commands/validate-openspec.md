---
description: Validate the openspec/ directory — structure, cross-references, SemVer alignment, scenario well-formedness.
allowed-tools: Bash(./build/openspec-lint.sh), Bash(./build/openspec-lint.ps1), Read, Agent(spec-reviewer)
---

Validate all OpenSpec artifacts.

Steps:
1. Run `./build/openspec-lint.sh` (or `.ps1` on Windows).
2. Invoke `spec-reviewer` agent for semantic checks.
3. Surface every BLOCK-severity issue.
4. Exit non-zero if any blocker remains.
