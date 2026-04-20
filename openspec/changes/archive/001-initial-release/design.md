# Design — 001-initial-release — Here.Sdk.Common

## Context

`Common` is a new, zero-dependency .NET Standard / .NET library that underpins the whole `Here.Sdk.*` ecosystem. It hosts the primitive vocabulary shared by every downstream package (`Abstractions`, `Core`, `Android`, `iOS`, `Forms`, `Bindings.*`). The prior HERE Premium Mobile SDK v3 is end-of-life; the new target is HERE SDK 4.x Navigate Edition, whose data model informs the ubiquitous language used here.

## Chosen approach

- **Value-object primitives** (immutable `record` / `record struct`) with invariants enforced at construction.
- **Zero runtime dependencies** — no Newtonsoft, no Microsoft.Extensions.*, no System.Reactive. Every abstraction we need stays inside BCL.
- **Multi-target** (`netstandard2.0;netstandard2.1;net8.0`) so Xamarin.Forms 5 and .NET MAUI 8 consumers can both reference the package without shims.
- **WGS84 as the canonical geodetic datum** — `GeoCoordinates` validates lat/lon bounds in the primary constructor; helpers assume WGS84 throughout.
- **Exception hierarchy** rooted at `HereException : Exception`, with specialised subclasses carrying structured auxiliary data (`HereRateLimitedException.RetryAfter`, `HereInvalidRequestException.FieldName`).
- **Flexible Polyline** (HERE format) round-trip encoding implemented in pure .NET without external codecs.
- **Coverage floor** at 90 % line + branch + 95 % method **globally and per file** — public API at 100 %; per-file enforcement via `coverlet.runsettings` and `build/coverage-gate.sh`.

## Trade-offs accepted

- No Reactive primitives here — pushed to `Abstractions`. Cost: consumers needing observable position streams will take a second dependency.
- Minimal API surface — accepting that downstream packages will add more (e.g., `Route`, `Maneuver`) rather than piling them into `Common`.
- Multi-target build requires `netstandard2.0` compromises (no `System.Text.Json.Nodes`, polyfills where needed). Benefit: Xamarin.Forms 5 compatibility.

## Risks and mitigations

- **Risk:** API expansion pressure from consumers pushing for "one more helper" creeping into `Common`. **Mitigation:** `package-api-reviewer` enforces scope; proposals adding new public surface must justify why `Common` is the right home vs. downstream package.
- **Risk:** Performance regression on hot paths (`GeoCoordinates.DistanceTo`, `FlexiblePolyline.Decode`) due to .NET Standard 2.0 polyfills. **Mitigation:** BenchmarkDotNet baselines per TFM; regression > 10 % blocks CI.
- **Risk:** Non-ASCII characters in test fixtures reproducing HERE place names (`Köln`, `São Paulo`) trip the cspell + non-ASCII identifier checks. **Mitigation:** per-file allow-list in `.cspell.json`; non-ASCII check scoped to `src/**/*.cs` (excludes tests and fixtures).

## Performance considerations

Hot paths annotated with `// PERF: hot path` and backed by BenchmarkDotNet baselines under `tests/Here.Sdk.Common.Benchmarks/baselines/`:

- `GeoCoordinates.DistanceTo(GeoCoordinates)` — Haversine formula; target ≤ 50 ns, 0 allocations.
- `GeoBearing(double)` constructor — target ≤ 5 ns, 0 allocations.
- `FlexiblePolyline.Decode(string)` on 1000-point input — target ≤ 5 ms, allocation proportional to output array.
- `FlexiblePolyline.Encode(IEnumerable<GeoCoordinates>)` on 1000 points — target ≤ 5 ms, one allocation.

Every hot-path method declares its allocation budget in the XML doc `<remarks>` and links to its benchmark.

## AOT / Trim friendliness

- Assemblies marked `<IsTrimmable>true</IsTrimmable>`.
- No dynamic reflection anywhere.
- No `Type.GetType(string)`, no `Assembly.Load`.
- Source generators allowed (none needed in `Common` v1.0).
- Verified by `<TrimmerSingleWarn>false</TrimmerSingleWarn>` and running `dotnet publish -c Release -r android-arm64 /p:PublishTrimmed=true` without warnings.

## Security considerations

- No credentials handled in `Common`.
- No I/O.
- No secrets possible — the repo-wide `detect-here-credentials.sh` runs on every commit anyway as a defence-in-depth.

## Observability

`Common` emits neither metrics nor traces — it is a pure library. Consumers that want to trace calls (e.g., routing latency) do so one layer up in `Core`.

## Migration / rollout

First public release. Rollout strategy:

1. `develop` accumulates commits; prereleases ship automatically as `0.1.0-alpha.N` on nuget.org.
2. Once the public API is frozen (tasks 3.1 – 3.5 complete + full coverage), a `feat!:` commit promotes to `1.0.0`.
3. Release Please opens the PR on `master`; merging the PR creates tag `v1.0.0` and triggers `nuget-publish.yml`.
4. Downstream sibling packages bump their `PackageReference` to `[1.0.0, 2.0.0)`.

Rollback path: `dotnet nuget delete Here.Sdk.Common <version>` within the 72 h window nuget.org permits deletions. Past that, publish a patch revoking the bad surface via `[Obsolete]` with a deprecation message.

## Observability of the proposal itself

This proposal is governed by `openspec-methodology/spec.md`. The `spec-reviewer` agent validates it on every PR touching `openspec/**`. The mandatory pre-PR gate (`./build/verify.sh`) invokes `openspec-lint` which is the executable version of the structural rules.

## References

- HERE SDK 4.x Navigate Edition (Android) — <https://www.here.com/docs/bundle/sdk-for-android-navigate-developer-guide>.
- HERE Flexible Polyline format — <https://github.com/heremaps/flexible-polyline>.
- Clean Architecture (Robert C. Martin) — inward-pointing dependency rule applied at the ecosystem level (see `coding-principles/spec.md`).
- BenchmarkDotNet — <https://benchmarkdotnet.org/>.
