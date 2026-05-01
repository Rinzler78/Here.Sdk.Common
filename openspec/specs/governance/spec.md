# Governance

## Purpose

Defines the branching strategy, commit conventions, code review gates, OpenSpec proposal
workflow, release automation, secret rotation, and hook set for `Here.Sdk.Common`.

## Requirements

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

### Requirement: Native branch-guard hooks independent of pre-commit

The system SHALL ship a `.githooks/` directory committed to source control, containing
`pre-commit`, `pre-push`, and `commit-msg` bash scripts. `git config core.hooksPath =
.githooks` activates them without requiring `pre-commit` to be installed. The
`pre-commit` script blocks commits on `master`, `develop`, and `release/*`. The
`pre-push` script blocks direct pushes to those branches and delegates to
`pre-commit run --hook-stage pre-push` when available.

#### Scenario: Commit on develop without pre-commit installed

- **WHEN** a developer without `pre-commit` runs `git commit` on `develop`
- **THEN** `.githooks/pre-commit` (native bash) detects the branch AND exits non-zero AND the commit is rejected

#### Scenario: Direct push to develop blocked locally

- **WHEN** a developer runs `git push origin develop` from a local branch
- **THEN** `.githooks/pre-push` exits non-zero AND the push is rejected before reaching GitHub

### Requirement: Worktree-based development workflow mandatory

The system SHALL mandate that every non-trivial development task (feature, fix, chore,
docs) uses a git worktree instead of a direct branch checkout on the main clone.
Command: `git worktree add .worktrees/<branch> -b <type>/<slug>`. The main clone
MUST NOT be used for feature work. `.worktrees/` SHALL be listed in `.gitignore`.

#### Scenario: Developer starts a new feature

- **WHEN** a developer begins work on `feat/my-feature`
- **THEN** they run `git worktree add .worktrees/feat-my-feature -b feat/my-feature` AND all work happens in that worktree AND the main clone stays on its current branch

### Requirement: Package restore phantom-dependency check at pre-push

The system SHALL enable `RestorePackagesWithLockFile=true` in `Directory.Build.props`
and commit all generated `packages.lock.json` files. The pre-push hook SHALL run
`dotnet restore --locked-mode --no-cache`, detecting any package referenced in source
but absent from a `.csproj`.

#### Scenario: Phantom dependency detected

- **WHEN** a developer uses a type from a package without declaring the `PackageReference`
- **THEN** `dotnet restore --locked-mode` fails AND the push is rejected

### Requirement: XML documentation completeness check at pre-push

The system SHALL run `dotnet build -c Release --warnaserror:CS1591 --no-incremental`
at pre-push stage. Every public type and member SHALL carry an XML `<summary>` comment.

#### Scenario: Missing XML doc on public type

- **WHEN** a developer adds a public type or member without a `<summary>` comment
- **THEN** CS1591 is emitted as an error AND the push is rejected

### Requirement: Coverage gate at pre-push

The system SHALL run `./build/coverage-gate.sh` at pre-push stage in addition to CI,
preventing coverage regressions from being pushed to the remote.

#### Scenario: Coverage drops below threshold

- **WHEN** a pushed change lowers line coverage below 90 %
- **THEN** `coverage-gate.sh` exits non-zero AND the push is rejected

### Requirement: Extended pre-commit hook set

`.pre-commit-config.yaml` SHALL include `cspell` (local node hook), `dotnet format
--verify-no-changes` (C# files only), `markdownlint`, and `yamllint`, in addition to
the baseline hooks (`codespell`, `detect-secrets`, `no-commit-to-branch`,
`conventional-pre-commit`, `trailing-whitespace`, etc.).

#### Scenario: Markdown lint error on commit

- **WHEN** a developer stages a `.md` file with a trailing space
- **THEN** `markdownlint` exits non-zero AND the commit is rejected

#### Scenario: YAML syntax error on commit

- **WHEN** a developer stages a `.yml` file with invalid indentation
- **THEN** `yamllint` exits non-zero AND the commit is rejected
