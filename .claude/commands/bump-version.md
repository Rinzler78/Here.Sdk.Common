---
description: Preview the next version bump based on Conventional Commits since the last tag, or explicitly bump to major/minor/patch. Does NOT modify version.json — Release Please owns that.
argument-hint: major | minor | patch | auto
allowed-tools: Bash(nbgv get-version:*), Bash(git log:*), Bash(git describe:*), Bash(gh release list:*), Read
---

Preview the next version for `$1` mode.

Steps:
1. Resolve last stable tag (`git describe --tags --match='v*' --abbrev=0`).
2. List commits since: `git log <last-tag>..HEAD --oneline`.
3. Classify via Conventional Commits → compute expected bump.
4. If `$1` = `auto`, use computed bump.
5. If `$1` ∈ {`major`, `minor`, `patch`}, override with user intent and warn if Conventional Commits say otherwise.
6. Print current version, next version, and justification.
7. Do NOT write `version.json` — Release Please owns the bump.
