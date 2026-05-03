# Delta — governance (MODIFIED)

## ADDED Requirements

### Requirement: Native branch-guard hooks independent of pre-commit

The system SHALL ship a `.githooks/` directory committed to source control, containing
`pre-commit`, `pre-push`, and `commit-msg` bash scripts. `git config core.hooksPath =
.githooks` activates them without requiring `pre-commit` to be installed. The
`pre-commit` script blocks commits on `master`, `develop`, and `release/*`. The
`pre-push` script blocks direct pushes to those branches and delegates to
`pre-commit run --hook-stage pre-push` when available.

#### Scenario: Commit on develop without pre-commit installed

- **WHEN** a developer without `pre-commit` runs `git commit` on `develop`
- **THEN** `.githooks/pre-commit` (native bash) detects the branch AND exits non-zero
  AND the commit is rejected

#### Scenario: Direct push to develop blocked locally

- **WHEN** a developer runs `git push origin develop` from a local branch
- **THEN** `.githooks/pre-push` exits non-zero AND the push is rejected before
  reaching GitHub

### Requirement: Worktree-based development workflow mandatory

The system SHALL mandate that every non-trivial development task (feature, fix, chore,
docs) uses a git worktree instead of a direct branch checkout on the main clone.
Command: `git worktree add .worktrees/<branch> -b <type>/<slug>`. The main clone
MUST NOT be used for feature work. `.worktrees/` SHALL be listed in `.gitignore`.

#### Scenario: Developer starts a new feature

- **WHEN** a developer begins work on `feat/my-feature`
- **THEN** they run `git worktree add .worktrees/feat-my-feature -b feat/my-feature`
  AND all work happens in that worktree AND the main clone stays on its current branch

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
