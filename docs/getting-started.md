# Getting Started with Here.Sdk.Common

## Installation

```xml
<PackageReference Include="Here.Sdk.Common" Version="1.0.0" />
```

Or via the .NET CLI:

```bash
dotnet add package Here.Sdk.Common
```

## Basic Usage

```csharp
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Units;

// Create geographic coordinates
var paris = new GeoCoordinates(48.8566, 2.3522);
var london = new GeoCoordinates(51.5074, -0.1278);

// Compute distance between two points
var polyline = new GeoPolyline(new[] { paris, london });
var distance = polyline.Length();
Console.WriteLine($"Paris → London: {distance.ToKilometers():F1} km");

// Use unit system preference
var system = UnitSystem.Metric;
```

## Requirements

- .NET Standard 2.0, 2.1, or .NET 8.0
- No runtime HERE credentials required — credentials are only needed by downstream `Here.Sdk.Core` packages.

## Next Steps

- [Credentials Setup](credentials-setup.md) — required only for `Here.Sdk.Core` or higher.
- [Architecture Decision Records](architecture/decision-records/) — design rationale.
