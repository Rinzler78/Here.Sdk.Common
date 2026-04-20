---
description: Create a new Architecture Decision Record under docs/architecture/decision-records/.
argument-hint: "Title in quotes"
allowed-tools: Read, Write, Bash(ls:*)
---

Create `docs/architecture/decision-records/ADR-NNNN-<slug>.md`.

Steps:
1. List existing ADRs to compute next number: `ls docs/architecture/decision-records/ADR-*.md | sort | tail -1`.
2. Slugify the title (kebab-case, ASCII).
3. Write the ADR from template:

```markdown
# ADR-NNNN — <Title>

**Status:** proposed
**Date:** YYYY-MM-DD
**Deciders:** @rinzler78

## Context

## Decision

## Consequences

- Positive:
- Negative:
- Neutral:

## Alternatives considered

- Option A: …
- Option B: …

## References
```

4. Output the new file path.
