# Delta — testing (ADDED)

## ADDED Requirements

### Requirement: Hard coverage gate at 90 % line + 90 % branch + 95 % method globally AND per file
The system SHALL enforce, in CI and pre-commit, that every `src/**/*.cs` production file achieves ≥ 90 % line coverage, ≥ 90 % branch coverage, ≥ 95 % method coverage. Aggregate coverage across the repo SHALL also meet these thresholds. Public API surface SHALL reach 100 %.

#### Scenario: CI fails if a single file is below threshold
- **WHEN** a PR introduces a `src/**/*.cs` file with 80 % line coverage
- **THEN** the `coverage-gate` CI step exits non-zero AND the PR is blocked from merging

#### Scenario: CI passes when all files meet threshold
- **WHEN** every production file is ≥ 90 % line, ≥ 90 % branch, ≥ 95 % method
- **THEN** the `coverage-gate` step exits 0

#### Scenario: Regression tolerance
- **WHEN** aggregate coverage drops by more than 0.5 % between the base branch and the PR
- **THEN** the `coverage-gate` step fails with a diff report


### Requirement: Coverage exemptions only via explicit annotation
The system SHALL exempt a file from per-file coverage only if it carries `[ExcludeFromCodeCoverage]` or is listed in `coverlet.runsettings` under `<ExcludeByFile>` or `<ExcludeByAttribute>`.

#### Scenario: Generated file excluded automatically
- **WHEN** a file under `src/**/Generated/` is measured
- **THEN** it is excluded from the coverage gate

#### Scenario: Manual exemption requires explicit attribute
- **WHEN** a hand-written file is added without `[ExcludeFromCodeCoverage]` and without a `<ExcludeByFile>` entry
- **THEN** the coverage gate applies normally


### Requirement: Deterministic unit tests
The system SHALL require every unit test to be hermetic and deterministic: no network I/O, no filesystem I/O outside `Path.GetTempPath()`, no `DateTime.Now` / `DateTimeOffset.UtcNow`, no `Thread.Sleep`, no `Random` without a seed.

#### Scenario: Production code using DateTime.Now is flagged
- **WHEN** a `src/**/*.cs` file contains `DateTime.Now` or `DateTimeOffset.UtcNow` not behind an injected `TimeProvider`
- **THEN** the `code-reviewer` agent blocks the PR

#### Scenario: Unit test using Task.Delay is flagged
- **WHEN** a `tests/**/UnitTests/*.cs` file contains `Task.Delay`
- **THEN** the `test-strategist` agent flags it as a BLOCK finding


### Requirement: Integration tests skip cleanly without HERE credentials
The system SHALL mark every test requiring HERE credentials with `[SkippableFact]` + `Skip.IfNot(hasCredentials, reason)`, so PRs from forks that cannot access secrets skip those tests.

#### Scenario: Credentials absent ⇒ test skipped, not failed
- **WHEN** a credentialled `[SkippableFact]` runs without `HERE_ACCESS_KEY_ID` set
- **THEN** the test result is `Skipped` with a clear message AND CI does not fail

#### Scenario: Credentials present ⇒ test runs
- **WHEN** `HERE_ACCESS_KEY_ID` and `HERE_ACCESS_KEY_SECRET` are set
- **THEN** the test executes and reports Pass/Fail


### Requirement: Test organisation and naming
The system SHALL organise tests under `tests/Here.Sdk.Premium.Common.UnitTests/` with one test class per source class, named `Method_Condition_ExpectedOutcome` for every method.

#### Scenario: Every public source member has at least one test
- **WHEN** the package is built
- **THEN** a dedicated test method targeting each public member exists in the test project


### Requirement: Mutation testing baseline (non-blocking v1)
The system SHALL run Stryker.NET nightly against `Abstractions` + `Common` + `Core` capability code, with a mutation score target ≥ 75 %; the result is non-blocking in v1 but published on the CI dashboard.

#### Scenario: Nightly mutation run produces a report
- **WHEN** the nightly Stryker workflow executes
- **THEN** a `StrykerOutput/reports/mutation-report.html` artifact is uploaded to the run


### Requirement: Enum files excluded from per-file coverage gate
The system SHALL exclude pure enum files (files containing only `enum` declarations with no methods, properties, or validation logic) from the per-file coverage gate. The exclusion SHALL be declared via `<ExcludeByFile>` in `coverlet.runsettings` matching the pattern `**/src/**/*Enum*.cs` AND via directory (`src/Transport/`, `src/Navigation/`, `src/Traffic/`, `src/Ev/`, `src/Map/`, `src/Search/`, `src/Routing/` enum files).

#### Scenario: Pure enum file not reported as under-covered
- **WHEN** `coverage-gate.sh` runs against a repo containing only enum declarations in `src/Transport/TransportMode.cs`
- **THEN** the file does not appear in the per-file threshold report

#### Scenario: Enum file with helper method IS covered
- **WHEN** an enum file contains a static extension method
- **THEN** the coverage gate applies normally to that extension method


### Requirement: Ordinal stability tests for all stable enums
The system SHALL provide, in `tests/Here.Sdk.Premium.Common.UnitTests/Enums/`, one test class per enum that asserts the ordinal values of the first member and any documented stable sentinel members.

#### Scenario: First member ordinal asserted
- **WHEN** the test suite runs
- **THEN** `Assert.Equal(0, (int)TransportMode.Car)` passes AND `Assert.Equal(0, (int)CardinalDirection.North)` passes (and similar for every enum)

#### Scenario: Flags enum composition asserted
- **WHEN** `HazardousMaterial.Explosive | HazardousMaterial.Gas` is evaluated
- **THEN** `(result & HazardousMaterial.Explosive) != 0` AND `(result & HazardousMaterial.Gas) != 0`


### Requirement: Value record snapshot tests
The system SHALL snapshot-test every value record with more than 3 fields using `Verify` (VerifyTests NuGet) to prevent accidental property additions/removals from going unnoticed.

#### Scenario: Snapshot fails when a property is added
- **WHEN** a new property is added to `Location` without updating the snapshot
- **THEN** the snapshot test fails with a diff showing the new property

#### Scenario: Snapshot passes after update
- **WHEN** the snapshot is updated via `dotnet test -- Verify.UseDirectory=artifacts/snapshots`
- **THEN** the test passes on the next run
