# Delta — openspec-methodology (ADDED)

## ADDED Requirements

### Requirement: Repository structure for OpenSpec
The system SHALL organise OpenSpec artifacts as:
- `openspec/project.md` — repo identity.
- `openspec/tech.md` — tech stack + ADR pointers.
- `openspec/AGENTS.md` — Claude agent routing.
- `openspec/README.md` — 60-second tour.
- `openspec/specs/<capability>/spec.md` — CURRENT merged capabilities.
- `openspec/changes/<change-id>/{proposal.md,tasks.md,design.md,specs/<capability>/spec.md}` — proposed changes in flight.
- `openspec/changes/archive/<change-id>/` — landed and archived proposals.

#### Scenario: Missing project.md
- **WHEN** `./build/openspec-lint.sh` runs on a repo missing `openspec/project.md`
- **THEN** it exits non-zero with message `MISSING_PROJECT_FILE`


### Requirement: `proposal.md` YAML frontmatter and required sections
The system SHALL require every `proposal.md` to carry a YAML frontmatter with fields `id`, `title`, `status` (one of `draft`, `in-review`, `approved`, `merged`, `archived`, `rejected`), `author`, `created` (ISO-8601 date), `target-specs` (list of paths), `semver-impact` (one of `MAJOR`, `MINOR`, `PATCH`, `NONE`), AND to contain H2 sections `Why`, `What changes`, `Impact`, `Alternatives considered`.

#### Scenario: Missing frontmatter field
- **WHEN** a `proposal.md` lacks `semver-impact`
- **THEN** `spec-reviewer` emits a BLOCK finding `FRONTMATTER_MISSING_SEMVER`

#### Scenario: Thin Why section
- **WHEN** the `Why` section contains fewer than 30 non-whitespace characters
- **THEN** `spec-reviewer` emits BLOCK `THIN_MOTIVATION`


### Requirement: `tasks.md` strict numbering and acceptance criteria
The system SHALL require every task line in `tasks.md` to:
- Start with `- [ ]` or `- [x]`.
- Carry an ID in the form `N.M` (phase.number).
- Include an `**Acceptance**:` line describing the observable definition of done.
- Include a `**Verification**:` line describing how "done" is proven.
- Include a `**Coverage**:` line with a percentage whenever the task references `src/**/*.cs`.
AND SHALL include a `Summary` block with `Total tasks`, `Est. effort`, `Blocked by`, `Blocks`.

#### Scenario: Task missing Acceptance
- **WHEN** a task has Verification but no Acceptance
- **THEN** `openspec-lint` exits non-zero with finding `TASK_MISSING_ACCEPTANCE`

#### Scenario: Task touching src/ without Coverage
- **WHEN** a task references a file under `src/` but lacks a `Coverage:` line
- **THEN** `openspec-lint` exits non-zero with finding `TASK_MISSING_COVERAGE`


### Requirement: Delta specs use ADDED/MODIFIED/REMOVED/RENAMED sections with Requirement + Scenario
The system SHALL require every delta spec under `openspec/changes/<id>/specs/<capability>/spec.md` to:
- Contain at least one of the four section headers `## ADDED Requirements`, `## MODIFIED Requirements`, `## REMOVED Requirements`, `## RENAMED Requirements`.
- Under each section, every `### Requirement:` heading SHALL be followed by a SHALL / MUST statement AND at least one `#### Scenario:` block.
- Every `#### Scenario:` block SHALL contain a `- **WHEN**` bullet AND a `- **THEN**` bullet.

#### Scenario: Requirement without Scenario
- **WHEN** a delta spec has `### Requirement: X` without any `#### Scenario:`
- **THEN** `openspec-lint` exits non-zero with finding `REQUIREMENT_WITHOUT_SCENARIO`

#### Scenario: Scenario without WHEN/THEN
- **WHEN** a `#### Scenario:` block has only a description and no WHEN/THEN bullets
- **THEN** `openspec-lint` exits non-zero with finding `SCENARIO_MALFORMED`


### Requirement: SemVer alignment between proposal and delta
The system SHALL require `proposal.md.frontmatter.semver-impact` to match the delta type:
- Pure `ADDED` → at least `MINOR` (or `MAJOR` if the addition mandates new dependencies that break composition).
- Any `MODIFIED` or `REMOVED` of observable behavior → `MAJOR`.
- Pure `RENAMED` → `MAJOR`.
- Docs / tooling / internal only → `NONE`.
Mismatches SHALL be blocked by `spec-reviewer`.

#### Scenario: Declared PATCH on an ADDED requirement
- **WHEN** a proposal declares `semver-impact: PATCH` while delta contains `## ADDED Requirements`
- **THEN** `spec-reviewer` BLOCK `SEMVER_MISMATCH` with suggestion `MINOR`

#### Scenario: Declared MINOR on a MODIFIED behaviour
- **WHEN** a proposal declares `MINOR` while delta contains `## MODIFIED Requirements` altering observable behaviour
- **THEN** `spec-reviewer` BLOCK `SEMVER_MISMATCH` with suggestion `MAJOR`


### Requirement: Archive rule
The system SHALL require every merged proposal to be moved to `openspec/changes/archive/<id>/` with frontmatter `status: archived` on the same PR that applies the delta to `openspec/specs/`, preserving git history (no squash of the move).

#### Scenario: Merge without archive
- **WHEN** a proposal is merged but remains under `openspec/changes/<id>/` (not archived)
- **THEN** the next `openspec-lint` run emits WARN `PROPOSAL_NOT_ARCHIVED`


### Requirement: Spec-reviewer verdict format
The system SHALL require `spec-reviewer` to emit a structured verdict (`APPROVE` / `REQUEST_CHANGES` / `BLOCK`) listing findings by severity (BLOCK, WARN, INFO) with concrete line references.

#### Scenario: Spec-reviewer finds a structural issue
- **WHEN** a PR touching `openspec/**` is opened with a malformed `tasks.md`
- **THEN** `spec-reviewer` posts a report including the finding code (e.g., `TASK_MISSING_ACCEPTANCE`) AND line references
