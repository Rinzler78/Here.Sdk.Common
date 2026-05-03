# Delta — target-frameworks (MODIFIED)

## MODIFIED Requirements

### Requirement: Supported target frameworks include net9.0

`Here.Sdk.Common.csproj` SHALL list `net9.0` in `<TargetFrameworks>` alongside
`netstandard2.0`, `netstandard2.1`, and `net8.0`. The CI matrix SHALL run build
and test against `net9.0`.

#### Scenario: net9.0 TFM present in csproj

- **WHEN** a developer inspects `Here.Sdk.Common.csproj`
- **THEN** `<TargetFrameworks>` includes `net9.0`

#### Scenario: CI runs against net9.0

- **WHEN** a commit is pushed and CI runs
- **THEN** the build and test steps execute against the `net9.0` target AND succeed
