# Delta — coding-principles (ADDED)

## ADDED Requirements

### Requirement: English-only natural language across every artifact
The system SHALL enforce English (en-US) as the language for source identifiers, XML documentation, comments, OpenSpec documents, commit messages, PR descriptions, issues, and script help text. Enforcement SHALL occur via `cspell` + `codespell` pre-commit and CI hooks, and via a non-ASCII identifier CI check scanning `src/**/*.cs`.

#### Scenario: French identifier in src blocks PR
- **WHEN** a PR introduces `src/Foo/Données.cs` with a non-ASCII identifier
- **THEN** the non-ASCII identifier CI step exits non-zero AND the PR is blocked

#### Scenario: Common typo blocks commit
- **WHEN** a file staged for commit contains the word `recieve`
- **THEN** the `codespell` pre-commit hook fails AND the commit is rejected

#### Scenario: Valid project-specific term (e.g., `heresdk`) passes
- **WHEN** the word `heresdk` appears in a Markdown file
- **THEN** `cspell` accepts it via the `.cspell/here-ecosystem.txt` dictionary


### Requirement: Clean Code — functions small, intention-revealing, single-purpose
The system SHALL enforce, via code review and analyzers, that every function:
- Has an intention-revealing name (verb-first, no abbreviations).
- Fits within 20 lines (hard ceiling 40 with PR justification).
- Does exactly one thing (single level of abstraction).
- Accepts at most 3 parameters (a 4th requires a parameter object / record).
- Avoids flag parameters (split into two functions instead).

#### Scenario: Function with a boolean flag parameter is flagged
- **WHEN** a public method `CreateFile(bool temporary)` is reviewed
- **THEN** `code-reviewer` marks it BLOCK with suggestion to split into `CreateFile()` and `CreateTemporaryFile()`


### Requirement: Clean Architecture — inward dependency rule
The system SHALL enforce that `Here.Sdk.Premium.Common` has zero runtime dependencies on external NuGet packages and never references `Here.Sdk.Premium.Abstractions`, `Here.Sdk.Premium.Core`, or any downstream sibling. The dependency flow is strictly inward: outer layers depend on inner layers, never the reverse.

#### Scenario: Reference to Abstractions added
- **WHEN** a PR introduces `<PackageReference Include="Here.Sdk.Premium.Abstractions" … />` in any `Common` csproj
- **THEN** `build/clean-architecture-check.sh` exits non-zero AND the PR is blocked

#### Scenario: External NuGet dependency added
- **WHEN** a PR introduces a `PackageReference` to any non-BCL package
- **THEN** `build/clean-architecture-check.sh` flags it AND requires an ADR before merge


### Requirement: SOLID enforced via analyzers and review
The system SHALL enforce SOLID principles:
- **SRP** — classes ≤ 300 LoC, one reason to change.
- **OCP** — concrete classes `sealed` by default; extension via interfaces.
- **LSP** — contract tests verify every subtype honours its parent's contract.
- **ISP** — interfaces ≤ 7 members; larger interfaces must be split.
- **DIP** — collaboration only via abstractions; concrete types `internal`.

#### Scenario: Interface with 10 members
- **WHEN** a PR introduces `IEverything` with 10 methods
- **THEN** `code-reviewer` blocks with a suggestion to split into focused interfaces


### Requirement: Value objects encapsulate invariants at construction
The system SHALL model domain primitives as immutable records with invariants enforced in the primary constructor. Mutable setters, public properties without invariants, and primitive-obsession-style `string` / `double` in parameter lists are forbidden on the public surface.

#### Scenario: Primitive-obsession API introduced
- **WHEN** a PR adds `public void SetBearing(double degrees)`
- **THEN** `code-reviewer` blocks and suggests `public void SetBearing(GeoBearing bearing)`


### Requirement: No singletons, no service locator, constructor injection only
The system SHALL forbid public singletons (static `Instance` properties), service-locator patterns (`IServiceProvider.GetService<T>()` outside the composition root), and setter injection. Constructor injection is the only form of DI allowed.

#### Scenario: Public singleton added
- **WHEN** a PR adds `public static HereCalculator Instance { get; } = new();`
- **THEN** `code-reviewer` blocks


### Requirement: Async/await everywhere I/O happens, with CancellationToken
The system SHALL require every public async method to accept a `CancellationToken` parameter and propagate it. Synchronous bridges via `.Result`, `.Wait()`, `.GetAwaiter().GetResult()` are forbidden.

#### Scenario: `.Result` usage introduced
- **WHEN** a PR introduces `FooAsync().Result`
- **THEN** `Microsoft.VisualStudio.Threading.Analyzers` emits VSTHRD002 AND build fails in Release


### Requirement: Structured logging only (no Console.WriteLine)
The system SHALL forbid `Console.WriteLine` / `Debug.Write` / `Trace.Write` in `src/`. Logging SHALL route through `Microsoft.Extensions.Logging.Abstractions.ILogger<T>` with `LoggerMessage` source-generated methods to guarantee zero allocation.

#### Scenario: Console.WriteLine introduced in src
- **WHEN** a PR adds `Console.WriteLine("started")` in a `src/` file
- **THEN** `code-reviewer` blocks (though `Common` does not log; this rule bites when downstream packages apply the same contract)
