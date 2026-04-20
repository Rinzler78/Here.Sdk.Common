# Delta — governance (ADDED)

## ADDED Requirements

### Requirement: Two permanent branches with strict protection
The system SHALL maintain `master` (releasable) and `develop` (integration) as the only permanent branches. Both SHALL be protected via GitHub Rulesets: PR required, signed commits, linear history, conversation resolution, up-to-date required, force push blocked, deletion blocked.

#### Scenario: Direct push to master
- **WHEN** a maintainer attempts to push a commit directly to `master`
- **THEN** GitHub rejects the push with a ruleset violation

#### Scenario: Local commit attempt on master
- **WHEN** a developer runs `git commit` while on `master`
- **THEN** the `no-commit-to-branch` pre-commit hook exits non-zero AND the commit is rejected


### Requirement: Conventional Commits for every commit
The system SHALL enforce Conventional Commits via the `conventional-pre-commit` hook on the `commit-msg` stage.

#### Scenario: Non-conforming commit message
- **WHEN** a developer writes `git commit -m "updated stuff"`
- **THEN** the `commit-msg` hook exits non-zero AND the commit is rejected

#### Scenario: Conforming commit message
- **WHEN** a developer writes `git commit -m "feat: add GeoBearing value object"`
- **THEN** the commit succeeds


### Requirement: Review counts by target branch
The system SHALL require 1 PR approval to merge into `develop` and 2 PR approvals to merge into `master`. Authors SHALL NOT approve their own PRs. Breaking-change PRs (labeled `breaking-change`) SHALL require 2 approvals including the maintainer.

#### Scenario: Single approval on develop
- **WHEN** a PR to `develop` has 1 approval and CI green
- **THEN** the PR is mergeable

#### Scenario: Only 1 approval on master
- **WHEN** a PR to `master` has 1 approval
- **THEN** GitHub blocks merge until a second approval lands


### Requirement: OpenSpec proposal workflow for non-trivial changes
The system SHALL require any change that alters the public API, adds a NuGet dependency, changes a TFM, alters architecture, or introduces a new CI secret to be preceded by an OpenSpec proposal under `openspec/changes/<slug>/` containing `proposal.md`, `tasks.md`, a `design.md` when applicable, and delta specs.

#### Scenario: Public API PR without proposal
- **WHEN** a PR adds a new public type without a corresponding `openspec/changes/<slug>/` folder
- **THEN** `spec-reviewer` blocks with message `MISSING_PROPOSAL`


### Requirement: Release Please drives versioning and publishing
The system SHALL delegate tag creation, CHANGELOG maintenance, and version bumps to Release Please. Manual tag creation SHALL be detected and fail CI. `nuget-publish.yml` SHALL trigger on `release.published`.

#### Scenario: Manual tag push detected
- **WHEN** a maintainer pushes a tag `v1.2.3` not created by Release Please
- **THEN** `manual-tag-guard` CI job exits non-zero

#### Scenario: Release PR merged on master
- **WHEN** a Release Please PR is merged on `master`
- **THEN** a tag is created AND a GitHub Release is published AND `nuget-publish.yml` pushes the `.nupkg` to nuget.org


### Requirement: Secret rotation cadence
The system SHALL rotate CI secrets (`NUGET_API_KEY`, `HERE_ACCESS_KEY_ID`, `HERE_ACCESS_KEY_SECRET`, `HERE_API_KEY`, `AUTO_UPDATE_TOKEN`) every 90 days. Documented in `docs/credentials-setup.md`.

#### Scenario: Rotation past due
- **WHEN** a secret's `lastRotatedUtc` is older than 90 days
- **THEN** the nightly `secret-rotation-check` workflow opens a reminder issue
