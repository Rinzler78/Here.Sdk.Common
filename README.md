# Here.Sdk.Common

[![NuGet](https://img.shields.io/nuget/v/Here.Sdk.Common.svg)](https://www.nuget.org/packages/Here.Sdk.Common)
[![CI](https://github.com/rinzler78/Here.Sdk.Common/actions/workflows/ci.yml/badge.svg)](https://github.com/rinzler78/Here.Sdk.Common/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Zero-dependency primitive value objects, enums, and exceptions for the `Here.Sdk.*` ecosystem.

> **Community project** — not affiliated with HERE Technologies.

## Installation

```sh
dotnet add package Here.Sdk.Common
```

## Quick Start

```csharp
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Units;
using Here.Sdk.Common.Errors;

// Geographic coordinates (WGS84)
var paris = new GeoCoordinates(48.8566, 2.3522);
var berlin = new GeoCoordinates(52.5200, 13.4050);

// Polyline and distance
var route = new GeoPolyline([paris, berlin]);
double distanceMeters = route.Length();

// Units
var d = Distance.FromKilometers(100);
Console.WriteLine(d.ToMiles());

// Bearing (auto-normalized to [0, 360))
var bearing = new GeoBearing(-90); // → 270°

// Errors
try { /* HERE API call */ }
catch (HereRateLimitedException ex)
{
    Console.WriteLine($"Retry after: {ex.RetryAfter}");
}
```

## Features

- **Geography** — `GeoCoordinates`, `GeoBearing`, `GeoBoundingBox`, `GeoPolyline`, `GeoCircle`, `GeoPolygon`, `GeoCorridor`, `FlexiblePolyline`, `EarthConstants`
- **Units** — `Distance`, `Duration`, `Speed` with km/miles/kph/mph conversions
- **Errors** — `HereException` hierarchy (`HereNetworkException`, `HereAuthenticationException`, `HereRateLimitedException`, `HereInvalidRequestException`)
- **Geometry** — `Point2D/3D`, `Anchor2D`, `Size2D`, `Rectangle2D`, `Angle`, `AngleRange`, `IntegerRange`
- **Localization** — `LanguageCode`, `CountryCode`, `LocalizedText`, `LocalizedTexts`
- **~40 domain enums** — Transport, Routing, Search, Navigation, Traffic, EV, Map, Positioning

## Platforms

| Target | Min Version |
|--------|-------------|
| .NET Standard | 2.0, 2.1 |
| .NET | 8.0+ |
| Xamarin.Forms | 5.x (via netstandard2.0) |
| .NET MAUI | 8.0+ |

## Credentials

No HERE credentials are required to use this package. Credentials are needed by downstream `Here.Sdk.Core` packages. See [docs/credentials-setup.md](docs/credentials-setup.md).

## Documentation

- [Getting Started](docs/getting-started.md)
- [Architecture Decision Records](docs/architecture/decision-records/)
- [OpenSpec Proposal](openspec/changes/001-initial-release/proposal.md)

## Sample

```csharp
// Flexible Polyline encode/decode
var encoded = FlexiblePolyline.Encode([paris, berlin]);
var decoded = FlexiblePolyline.Decode(encoded);
```

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) and the [OpenSpec workflow](openspec/README.md) before submitting a PR.

## Security

See [SECURITY.md](SECURITY.md) for reporting vulnerabilities.

## License

MIT — see [LICENSE](LICENSE).

## Disclaimer

This is a community project and is not affiliated with, endorsed by, or supported by HERE Technologies. HERE® is a trademark of HERE Global B.V.
