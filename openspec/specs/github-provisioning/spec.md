# Delta — github-provisioning (ADDED)

## ADDED Requirements

### Requirement: GitHub repository created as public with canonical metadata
The system SHALL create the GitHub repository `Here.Sdk.Common` as a **public** repository under the `rinzler78` account, with: description matching the one-line elevator pitch from `project.md`, homepage pointing to the nuget.org package page, topics including `here`, `maps`, `nuget`, `dotnet`, `xamarin`, `maui`, `csharp`, License = MIT, wiki disabled, Discussions enabled, Issues enabled.

#### Scenario: Repository is public
- **WHEN** a maintainer queries `gh repo view rinzler78/Here.Sdk.Common --json visibility`
- **THEN** the `visibility` field equals `PUBLIC`

#### Scenario: Topics applied
- **WHEN** `gh repo view rinzler78/Here.Sdk.Common --json repositoryTopics` is read
- **THEN** the topics list includes `here`, `maps`, `nuget`, `dotnet`, `xamarin`, `maui`, `csharp`

#### Scenario: Wiki disabled, Discussions enabled
- **WHEN** `gh repo view rinzler78/Here.Sdk.Common --json hasWikiEnabled,hasDiscussionsEnabled` is read
- **THEN** `hasWikiEnabled` is `false` AND `hasDiscussionsEnabled` is `true`


### Requirement: Package-scoped NuGet API key
The system SHALL create a NuGet.org API key **scoped to the single package `Here.Sdk.Common`** with the action `Push new packages and package versions` and a 365-day expiry, and register it as the GitHub repository secret `NUGET_API_KEY`. The key SHALL NOT be reusable across other packages in the ecosystem.

#### Scenario: API key scope is single package
- **WHEN** the maintainer generates the key at `https://www.nuget.org/account/apikeys`
- **THEN** the key's `Package Owner` is the maintainer account AND the key's `Selected Packages` list contains exactly `Here.Sdk.Common`

#### Scenario: Key registered as GitHub secret
- **WHEN** `gh secret list --repo rinzler78/Here.Sdk.Common` is run
- **THEN** `NUGET_API_KEY` appears in the list AND its scope is `Repository`

#### Scenario: Rotation cadence documented
- **WHEN** a maintainer reads `docs/credentials-setup.md`
- **THEN** the rotation procedure for `NUGET_API_KEY` is documented with a 365-day cadence


### Requirement: Required GitHub secrets configured
The system SHALL configure the following secrets on the GitHub repository via `build/setup-github.sh`:

- `NUGET_API_KEY` (package-scoped, see requirement above).

No other secrets are required for this package.

#### Scenario: Secret list matches specification
- **WHEN** `gh secret list --repo rinzler78/Here.Sdk.Common` is run
- **THEN** exactly the secrets listed in this requirement are present


### Requirement: Branch protection Rulesets applied
The system SHALL apply GitHub Rulesets on `master` and `develop` with: PR required (1 approval for `develop`, 2 for `master`), required status checks (`ci / build-netstandard`, `ci / test`, `ci / pack`, `openspec-validate`), require conversation resolution, require up-to-date branches, block force push, block deletion, require signed commits, require linear history. An additional ruleset on `master` restricts direct updates to `release-please[bot]`.

#### Scenario: Direct push to master rejected
- **WHEN** a maintainer attempts to push a commit directly to `master`
- **THEN** the push is rejected with a ruleset violation message

#### Scenario: Unsigned commit rejected
- **WHEN** a PR contains a commit without a valid GPG or Sigstore signature
- **THEN** the ruleset blocks the merge

#### Scenario: Force push blocked
- **WHEN** a maintainer attempts `git push --force` to any protected branch
- **THEN** the operation is rejected


### Requirement: Environment `nuget-production` with protection
The system SHALL create a GitHub Environment named `nuget-production` protected by required reviewers (at least the maintainer) and restricted to deployment from the `master` branch only. The `nuget-publish.yml` workflow SHALL target this environment.

#### Scenario: Environment exists and is protected
- **WHEN** `gh api repos/rinzler78/Here.Sdk.Common/environments/nuget-production` is called
- **THEN** the response includes `protection_rules` with at least one `required_reviewers` entry AND `deployment_branch_policy` restricted to `master`


### Requirement: `build/setup-github.sh` is idempotent
The system SHALL provide `build/setup-github.sh` and `build/setup-github.ps1` that create / update the GitHub repository, apply Rulesets, set secrets, create the `nuget-production` environment, and configure Dependabot. The scripts SHALL be idempotent: re-running them against an already-provisioned repository SHALL report no changes.

#### Scenario: Re-running on provisioned repo reports no changes
- **WHEN** `./build/setup-github.sh` runs twice in a row against the same repo
- **THEN** the second run reports `No changes applied` AND exits 0


### Requirement: `docs/credentials-setup.md` documents every credential
The system SHALL document in `docs/credentials-setup.md` how to obtain, configure, and rotate every secret listed above, including step-by-step commands.

#### Scenario: Document covers every secret
- **WHEN** a contributor reads `docs/credentials-setup.md`
- **THEN** each secret configured on the repo has a dedicated section with rotation instructions
