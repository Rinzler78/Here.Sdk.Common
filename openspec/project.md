# Project — Here.Sdk.Common

## Identity

- **Package:** `Here.Sdk.Common`
- **Repository:** `github.com/rinzler78/Here.Sdk.Common`
- **TFM:** `netstandard2.0;netstandard2.1;net8.0`
- **License:** MIT — community project, not affiliated with HERE Technologies.

## Mission

Provide the minimal shared vocabulary used by every other package in the `Here.Sdk.*` ecosystem: geographic coordinates, distance/duration/speed units, a unified error taxonomy, and pure geometric helpers. Zero runtime dependencies, IO, or platform concerns.

## In scope

- Immutable value objects for geography (`GeoCoordinates`, `GeoBoundingBox`, `GeoPolyline`, `GeoBearing`).
- Unit types (`Distance`, `Duration`, `Speed`) with idiomatic conversions.
- Error taxonomy (`HereErrorCode`, `HereException` + specialized subclasses).
- Conversion helpers (WGS84 constants, flexible-polyline encoding/decoding).

## Non-goals

- No REST client, no networking (lives in `Here.Sdk.Core`).
- No platform types (Java, ObjC, UIKit, Android).
- No UI primitives.
- No `IObservable<T>` contracts (those belong in `Here.Sdk.Abstractions`).

## Target consumers

.NET developers building mobile (Xamarin.Forms 5, .NET MAUI 8) or server-side applications consuming HERE Maps data. No HERE credentials required to use this specific package.

## Stakeholders

- **Maintainer:** @rinzler78
- **Support:** GitHub Issues + Discussions on this repo.

## Current state

Greenfield — no public API has been merged yet. All requirements live in the in-flight proposal `openspec/changes/001-initial-release/`. Once that proposal lands, the delta spec content is copied to `openspec/specs/` as the stable contract.

## Related

- [`tech.md`](./tech.md)
- [`AGENTS.md`](./AGENTS.md)
- [`README.md`](./README.md)
- [`changes/001-initial-release/proposal.md`](./changes/001-initial-release/proposal.md)
