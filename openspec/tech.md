# Tech — Here.Sdk.Premium.Common

## Stack

| Layer | Tool |
|---|---|
| .NET SDK | 8.0.400+ pinned in `global.json` |
| Language | C# 12, nullable enabled, file-scoped namespaces |
| TFM | `netstandard2.0;netstandard2.1;net8.0` |
| Versioning | Nerdbank.GitVersioning + Release Please |
| Commits | Conventional Commits |
| Local hooks | pre-commit (Python, pipx) |
| CI | GitHub Actions — `ubuntu-latest` suffices (no platform-specific TFM) |
| Package feed | nuget.org (public) |
| Docs | DocFX + ADRs |
| Central Package Management | `Directory.Packages.props` |
| Analyzers | `Microsoft.CodeAnalysis.NetAnalyzers`, `StyleCop.Analyzers`, `Meziantou.Analyzer`, `Roslynator`, `SonarAnalyzer.CSharp`, `Microsoft.VisualStudio.Threading.Analyzers`, `AsyncFixer`, `Microsoft.CodeAnalysis.PublicApiAnalyzers` |
| Tests | xUnit + FluentAssertions + Moq + coverlet.collector |
| Spellcheck | cspell + codespell (English only) |
| Mutation testing | Stryker.NET (nightly, non-blocking v1) |

## Architectural decisions (ADR pointers, authored during initial release)

- ADR-0001 — Multi-target netstandard2.0/2.1/net8.0.
- ADR-0002 — Zero runtime dependencies (no Newtonsoft, no Microsoft.Extensions.*).
- ADR-0003 — WGS84 as canonical geodetic datum.
- ADR-0004 — Immutable value objects via `record` / `record struct`.
- ADR-0005 — Exception hierarchy rooted at `HereException` with specialized subtypes.

ADRs live under `docs/architecture/decision-records/` (authored by the initial-release proposal).

## Non-functional targets

- Local build (all TFMs Debug + Release): ≤ 2 minutes.
- CI full pipeline: ≤ 5 minutes.
- Package size (compressed `.nupkg`): ≤ 100 KB.
- Coverage: ≥ 90 % line + 90 % branch + 95 % method globally and per file; 100 % on public API.
