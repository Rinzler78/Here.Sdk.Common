# Delta — package-api (ADDED)

## ADDED Requirements

### Requirement: `GeoCoordinates` immutable value object with WGS84 validation
The system SHALL provide `public readonly record struct GeoCoordinates(double Latitude, double Longitude, double? Altitude = null)` under namespace `Here.Sdk.Common.Geography` that validates WGS84 ranges in the primary constructor.

#### Scenario: Valid latitude and longitude are stored as-is
- **WHEN** a consumer constructs `new GeoCoordinates(48.8566, 2.3522)`
- **THEN** `Latitude` equals `48.8566` AND `Longitude` equals `2.3522` AND `Altitude` is null

#### Scenario: Latitude outside [-90, 90] is rejected
- **WHEN** a consumer constructs `new GeoCoordinates(91, 0)`
- **THEN** an `ArgumentOutOfRangeException` is thrown with parameter name `Latitude`

#### Scenario: Longitude outside [-180, 180] is rejected
- **WHEN** a consumer constructs `new GeoCoordinates(0, 200)`
- **THEN** an `ArgumentOutOfRangeException` is thrown with parameter name `Longitude`

#### Scenario: ToString uses invariant culture
- **WHEN** `new GeoCoordinates(48.8566, 2.3522).ToString()` is called under `CultureInfo.GetCultureInfo("fr-FR")`
- **THEN** the output uses dot as decimal separator (invariant culture)


### Requirement: `GeoBearing` normalised to `[0, 360)`
The system SHALL provide `public readonly record struct GeoBearing(double DegreesFromNorth)` that normalises input to `[0, 360)` in the primary constructor.

#### Scenario: Valid input stored as-is
- **WHEN** `new GeoBearing(45)` is constructed
- **THEN** `DegreesFromNorth` equals `45`

#### Scenario: Negative input wraps positive
- **WHEN** `new GeoBearing(-90)` is constructed
- **THEN** `DegreesFromNorth` equals `270`

#### Scenario: Overflow input wraps via modulo 360
- **WHEN** `new GeoBearing(450)` is constructed
- **THEN** `DegreesFromNorth` equals `90`

#### Scenario: Equality across equivalent normalised inputs
- **WHEN** `new GeoBearing(45)` and `new GeoBearing(-315)` are compared
- **THEN** they compare equal via `==` AND have identical `GetHashCode()`


### Requirement: `GeoBoundingBox` with SW ≤ NE invariant and antimeridian support
The system SHALL provide `public readonly record struct GeoBoundingBox(GeoCoordinates SouthWest, GeoCoordinates NorthEast)` that enforces `SouthWest.Latitude ≤ NorthEast.Latitude` and handles antimeridian crossing.

#### Scenario: Latitude invariant violated
- **WHEN** `new GeoBoundingBox(new GeoCoordinates(10, 0), new GeoCoordinates(5, 10))` is constructed
- **THEN** an `ArgumentException` is thrown with message containing `Latitude`

#### Scenario: Contains returns true for an interior point
- **WHEN** `box.Contains(new GeoCoordinates(47, 2))` is called on a box from `(45, 0)` to `(50, 5)`
- **THEN** the result is `true`

#### Scenario: Antimeridian-crossing box accepts longitudes east of SW
- **WHEN** a box from `SouthWest(0, 170)` to `NorthEast(5, -170)` queries `Contains(GeoCoordinates(2, 175))`
- **THEN** the result is `true`


### Requirement: `GeoPolyline` immutable vertex sequence
The system SHALL provide `public sealed record GeoPolyline(IReadOnlyList<GeoCoordinates> Vertices)` with `Length()` and `Encode()` methods.

#### Scenario: Empty polyline reports zero length
- **WHEN** `GeoPolyline.Empty.Length()` is called
- **THEN** the result equals `Distance.Zero`

#### Scenario: Length sums Haversine segment distances
- **WHEN** `Length()` is called on a polyline of `N` vertices
- **THEN** the result equals the sum of `Vertices[i].DistanceTo(Vertices[i+1])` for `i ∈ [0, N-2]`


### Requirement: `FlexiblePolyline` encode/decode per HERE format
The system SHALL provide `public static class FlexiblePolyline` with `Encode(IEnumerable<GeoCoordinates>, byte precision = 5)` returning `string` and `Decode(string encoded)` returning `GeoPolyline`.

#### Scenario: Round-trip preserves vertices within precision
- **WHEN** a polyline of `N` coordinates is encoded then decoded with default precision 5
- **THEN** each decoded coordinate equals the original within `1e-5` absolute tolerance on both Latitude and Longitude

#### Scenario: Invalid input raises HereInvalidRequestException
- **WHEN** `Decode("not-a-valid-polyline")` is called
- **THEN** a `HereInvalidRequestException` is thrown with `Code == HereErrorCode.InvalidRequest`


### Requirement: `Distance`, `Duration`, `Speed` unit value objects
The system SHALL provide under `Here.Sdk.Common.Units`:
- `public readonly record struct Distance(double Meters)` with `Zero`, `FromKilometers(double)`, `FromMiles(double)`, `ToKilometers()`, `ToMiles()`, arithmetic operators `+`, `-`, `*` (scalar).
- `public readonly record struct Duration(TimeSpan Value)` wrapping `TimeSpan`.
- `public readonly record struct Speed(double MetersPerSecond)` with `ToKph()`, `ToMph()`.

#### Scenario: Distance round-trip km conversion
- **WHEN** `Distance.FromKilometers(1.5).ToKilometers()` is called
- **THEN** the result equals `1.5` within `1e-9`

#### Scenario: Distance arithmetic composes
- **WHEN** `Distance.FromKilometers(1) + Distance.FromKilometers(2)` is evaluated
- **THEN** the result's `ToKilometers()` equals `3.0`

#### Scenario: Speed conversion to kph
- **WHEN** `new Speed(10).ToKph()` is called
- **THEN** the result equals `36.0` within `1e-9`


### Requirement: `HereErrorCode` enum and `HereException` hierarchy
The system SHALL provide under `Here.Sdk.Common.Errors`:
- `public enum HereErrorCode { None, NetworkFailure, AuthenticationFailure, InvalidRequest, NotFound, RateLimited, QuotaExceeded, Timeout, Cancelled, Unknown }`.
- `public class HereException : Exception { public HereErrorCode Code { get; } }`.
- `public sealed class HereNetworkException : HereException`.
- `public sealed class HereAuthenticationException : HereException`.
- `public sealed class HereRateLimitedException : HereException { public TimeSpan? RetryAfter { get; } }`.
- `public sealed class HereInvalidRequestException : HereException { public string? FieldName { get; } }`.

#### Scenario: HereException carries code and message
- **WHEN** `new HereException("boom", HereErrorCode.NetworkFailure)` is constructed
- **THEN** `Message` equals `"boom"` AND `Code` equals `HereErrorCode.NetworkFailure`

#### Scenario: HereRateLimitedException carries RetryAfter
- **WHEN** `new HereRateLimitedException("throttled", TimeSpan.FromSeconds(30))` is constructed
- **THEN** `RetryAfter` equals `TimeSpan.FromSeconds(30)` AND `Code` equals `HereErrorCode.RateLimited`

#### Scenario: Exception type hierarchy respected
- **WHEN** a consumer catches `HereException`
- **THEN** every specialised subclass is caught


### Requirement: Thread safety via immutability
The system SHALL ensure every public type exposed by this package is thread-safe by being immutable.

#### Scenario: All public types marked readonly or sealed record
- **WHEN** the package is reflected at test time
- **THEN** every public struct is `readonly` AND every public record class is `sealed`


### Requirement: Nullable reference types enabled on public surface
The system SHALL expose all reference-type members with explicit nullability annotations.

#### Scenario: Public method with non-null input rejects null at runtime
- **WHEN** a consumer passes `null` to `FlexiblePolyline.Decode(null!)`
- **THEN** an `ArgumentNullException` is thrown


### Requirement: Geometry — `GeoCircle`, `GeoPolygon`, `GeoCorridor`, `GeoOrientation`
The system SHALL provide under `Here.Sdk.Common.Geography`:
- `public readonly record struct GeoCircle(GeoCoordinates Center, double RadiusInMeters)` — validates `RadiusInMeters >= 0`.
- `public sealed record GeoPolygon(IReadOnlyList<GeoCoordinates> Vertices)` — validates at least 3 vertices; vertices form a closed ring.
- `public sealed record GeoCorridor(GeoPolyline Polyline, int HalfWidthInMeters)` — validates `HalfWidthInMeters >= 0`.
- `public readonly record struct GeoOrientation(double? BearingInDegrees, double? TiltInDegrees, double? RollInDegrees)` — all values nullable; if present, Bearing normalised to `[0, 360)`, Tilt clamped to `[-90, 90]`, Roll to `[-180, 180]`.

#### Scenario: GeoCircle rejects negative radius
- **WHEN** `new GeoCircle(new GeoCoordinates(0, 0), -1)` is constructed
- **THEN** an `ArgumentOutOfRangeException` is thrown with parameter name `RadiusInMeters`

#### Scenario: GeoPolygon requires 3+ vertices
- **WHEN** `new GeoPolygon(new[] { new GeoCoordinates(0,0), new GeoCoordinates(1,1) })` is constructed
- **THEN** an `ArgumentException` is thrown with a message containing "at least 3 vertices"


### Requirement: 2D/3D geometry value objects
The system SHALL provide under `Here.Sdk.Common.Geometry`:
- `public readonly record struct Point2D(double X, double Y)`.
- `public readonly record struct Point3D(double X, double Y, double Z)`.
- `public readonly record struct Anchor2D(double HorizontalOffset, double VerticalOffset)` — offsets in `[0, 1]` range representing normalised 2D coordinates.
- `public readonly record struct Size2D(double Width, double Height)` — validates non-negative dimensions.
- `public readonly record struct Rectangle2D(Point2D Origin, Size2D Size)`.
- `public readonly record struct Angle(double ValueInDegrees)` with `static Angle FromRadians(double)` and `double ToRadians()`.
- `public readonly record struct AngleRange(Angle StartAngle, Angle Extent)`.
- `public readonly record struct IntegerRange(int Min, int Max)` — validates `Min <= Max`.

#### Scenario: Angle round-trip via radians
- **WHEN** `Angle.FromRadians(Math.PI).ValueInDegrees` is evaluated
- **THEN** the result equals `180.0` within `1e-9`

#### Scenario: IntegerRange rejects inverted bounds
- **WHEN** `new IntegerRange(5, 3)` is constructed
- **THEN** an `ArgumentException` is thrown with parameter name `Min`


### Requirement: `Location` value record
The system SHALL provide `public sealed record Location` in `Here.Sdk.Common.Positioning` with:
- **Required:** `GeoCoordinates Coordinates`, `DateTimeOffset Timestamp`.
- **Optional:** `double? BearingInDegrees`, `double? SpeedInMetersPerSecond`, `double? HorizontalAccuracyInMeters`, `double? VerticalAccuracyInMeters`, `double? BearingAccuracyInDegrees`, `double? SpeedAccuracyInMetersPerSecond`, `double? AltitudeAccuracyInMeters`.

#### Scenario: Minimal construction succeeds
- **WHEN** `new Location { Coordinates = new GeoCoordinates(0, 0), Timestamp = DateTimeOffset.UtcNow }` is constructed
- **THEN** all nullable fields are `null`

#### Scenario: Location equality by value
- **WHEN** two `Location` records have identical field values
- **THEN** they compare equal via `==` and share the same `GetHashCode()`


### Requirement: Localisation value objects
The system SHALL provide under `Here.Sdk.Common.Localisation`:
- `public readonly record struct LanguageCode(string Value)` — wraps ISO 639-1/639-3 language tag; validates non-empty.
- `public readonly record struct CountryCode(string Value)` — wraps ISO 3166-1 alpha-3; validates non-empty.
- `public sealed record LocalizedText(string Text, LanguageCode Language)`.
- `public sealed record LocalizedTexts(IReadOnlyList<LocalizedText> Items)` with `string? GetText(LanguageCode preferred)` returning the preferred language text or the first available.

#### Scenario: GetText returns preferred language
- **WHEN** `LocalizedTexts` contains entries for `en` and `fr` and `GetText(new LanguageCode("fr"))` is called
- **THEN** the French text is returned

#### Scenario: GetText falls back to first entry when preferred absent
- **WHEN** `LocalizedTexts` contains only `en` and `GetText(new LanguageCode("de"))` is called
- **THEN** the English text is returned


### Requirement: Identifier value objects
The system SHALL provide under `Here.Sdk.Common.Identifiers`:
- `public readonly record struct NameId(string Name, string Id)` — both non-empty.
- `public readonly record struct ExternalId(string Provider, string Id)` — both non-empty; `Provider` identifies the authority (e.g., `"here_place_id"`, `"wikidata"`).

#### Scenario: NameId equality
- **WHEN** two `NameId` instances share the same `Name` and `Id`
- **THEN** they compare equal via `==`


### Requirement: `UnitSystem` and `CardinalDirection` enums
The system SHALL provide under `Here.Sdk.Common.Units`:
- `public enum UnitSystem { Metric, Imperial }`.
- `public enum CardinalDirection { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }`.

#### Scenario: CardinalDirection ordinal stability
- **WHEN** `(int)CardinalDirection.North` is read
- **THEN** the value equals `0`


### Requirement: Transport-domain enums
The system SHALL provide under `Here.Sdk.Common.Transport`:
- `public enum TransportMode { Car, Truck, Pedestrian, Bicycle, Scooter, Taxi, Bus, PrivateBus, Ferry }` — supersedes the same enum currently drafted in Abstractions; Abstractions SHALL reference Common.
- `public enum VehicleType { Car, Truck, Bus, Motorcycle, Bicycle, Pedestrian }`.
- `public enum TruckType { Straight, Tractor }`.
- `public enum TruckCategory { Light, Medium, Heavy }`.
- `public enum TruckClass { Class1, Class2, Class3, Class4, Class5, Class6, Class7, Class8 }`.
- `public enum FuelType { Diesel, Petrol, LiquidPropaneGas, CompressedNaturalGas, LiquefiedNaturalGas, Hydrogen, Electric, HybridDiesel, HybridPetrol }`.
- `public enum TruckFuelType { Diesel, LiquidPropaneGas, CompressedNaturalGas, LiquefiedNaturalGas }`.
- `[Flags] public enum HazardousMaterial { None = 0, Explosive = 1, Gas = 2, Flammable = 4, Combustible = 8, Organic = 16, Poison = 32, Radioactive = 64, Corrosive = 128, PoisonousInhalation = 256, HarmfulToWater = 512, Other = 1024 }`.
- `public enum TunnelCategory { A, B, C, D, E }`.

#### Scenario: HazardousMaterial flags composable
- **WHEN** `HazardousMaterial.Explosive | HazardousMaterial.Gas` is evaluated
- **THEN** the result has both flags set AND `HasFlag(HazardousMaterial.Explosive)` returns `true`


### Requirement: Routing-domain enums
The system SHALL provide under `Here.Sdk.Common.Routing`:
- `public enum WaypointType { StopOver, PassThrough }`.
- `public enum OptimizationMode { Fastest, Shortest }`.
- `public enum ManeuverAction { Depart, Arrive, LeftUTurn, SharpLeftTurn, LeftTurn, SlightLeftTurn, Continue, SlightRightTurn, RightTurn, SharpRightTurn, RightUTurn, RoundaboutEnter, RoundaboutPass, RoundaboutExit51, RoundaboutExit52, RoundaboutExit53, RoundaboutExit54, RoundaboutExit55, RoundaboutExit56, RoundaboutExit57, RoundaboutExit58, RoundaboutExit59, RoundaboutExit510, RoundaboutExit511, RoundaboutExit512, Ferry, EnterHighway, LeaveHighway, BorderCrossing }` — mirrors `ManeuverAction` from HERE SDK.
- `public enum SectionTransportMode { Car, Truck, Pedestrian, Bicycle, Bus, Ferry, Scooter, Taxi, Train, TransitBus }`.
- `public enum NoticeSeverity { Critical, Warning, Info }`.
- `public enum IsolineRangeType { Distance, Time, ConsumptionInKilowattHours }`.
- `public enum PaymentMethod { AutomaticCoin, Cash, CreditCard, DiscountCard, NoPayment, PassSubscription, Transponder, TravelCard, VideoToll, Voucher }`.
- `public enum ChargingConnectorType { Unknown, IEC62196T1Combo, IEC62196T2, IEC62196T2Combo, IEC62196T3A, IEC62196T3C, Tesla, GBT20234Part2, GBT20234Part3, CHAdeMO, Domestic, Type1Combo, SaeJ3400 }`.
- `public enum ChargingSupplyType { Ac, Dc }`.
- `public enum RoutePlaceType { Unknown, Airport, Bus, CarTrain, Ferry, Hike, Park, PublicTransport, Start, Stop, End }`.

#### Scenario: ChargingConnectorType includes SAE_J3400 (NACS)
- **WHEN** `ChargingConnectorType.SaeJ3400` is referenced
- **THEN** the value compiles without error


### Requirement: Search-domain enums
The system SHALL provide under `Here.Sdk.Common.Search`:
- `public enum PlaceType { Unknown, Area, Locality, Street, HouseNumber, PointOfInterest }`.
- `public enum AddressType { Unknown, Point, HouseNumber, Street, PostalCode, LocalityLevel1, LocalityLevel2, LocalityLevel3, Country }`.
- `public enum DayOfWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }`.
- `public enum EvseStatus { Available, Occupied, OutOfService, Unknown }`.
- `public enum EvAccessType { Public, Restricted, Private, Test }`.

#### Scenario: DayOfWeek ordinal stability
- **WHEN** `(int)DayOfWeek.Monday` is read
- **THEN** the value equals `0`


### Requirement: Navigation-domain enums
The system SHALL provide under `Here.Sdk.Common.Navigation`:
- `public enum MilestoneType { StopOver, PassThrough, Waypoint, Destination }`.
- `public enum MilestoneStatus { Reached, Passed }`.
- `public enum SpeedWarningStatus { SpeedLimitExceeded, SpeedLimitRestored }`.
- `public enum RoadClassification { Unknown, Motorway, TrunkRoad, PrimaryRoad, SecondaryRoad, LocalRoad, PrivateRoad, Residential }`.
- `public enum LaneRecommendationState { NotRecommended, Recommended, HighlyRecommended }`.
- `public enum BorderCrossingType { CountryBorder, StateBorder }`.
- `public enum SafetyCameraType { Unknown, SpeedCamera, RedLightCamera, SectionCamera, MobileCamera }`.
- `public enum TollCollectionMethod { ExactCash, MixedCash, MultiLane, OpenRoad, BankCard }`.

#### Scenario: SpeedWarningStatus is exhaustive for switch
- **WHEN** a `switch` expression covers `SpeedLimitExceeded` and `SpeedLimitRestored`
- **THEN** the compiler reports no missing-pattern warning


### Requirement: Traffic-domain enums
The system SHALL provide under `Here.Sdk.Common.Traffic`:
- `public enum TrafficIncidentType { Unknown, Accident, Congestion, DisabledVehicle, MassTransit, Miscellaneous, OtherNews, PlannedEvent, RoadHazard, Construction, LaneRestriction, Weather }`.
- `public enum TrafficIncidentImpact { Unknown, Critical, Major, Minor, LowImpact }`.

#### Scenario: TrafficIncidentImpact severity ordering
- **WHEN** impacts are ordered as `Unknown < LowImpact < Minor < Major < Critical`
- **THEN** casting to int preserves this relative order


### Requirement: Positioning-domain enums
The system SHALL provide under `Here.Sdk.Common.Positioning`:
- `public enum LocationAccuracy { NavigationAccuracy, BestAvailable, BalancedPowerAccuracy, LowPowerAccuracy }`.

#### Scenario: Default accuracy
- **WHEN** `LocationAccuracy` is used without explicit value
- **THEN** `default(LocationAccuracy)` equals `NavigationAccuracy`


### Requirement: EV-domain enums
The system SHALL provide under `Here.Sdk.Common.Ev`:
- `public enum EvChargingConnectorFormat { Socket, Cable }`.
- `public enum EvseState { Available, Blocked, Charging, Inoperative, OutOfOrder, Planned, Removed, Reserved, Unknown }`.

#### Scenario: EvseState.Unknown is the safe default
- **WHEN** `default(EvseState)` is evaluated
- **THEN** the value is NOT `Available` — unknown is the zero-value to avoid false positives


### Requirement: Map-domain enums
The system SHALL provide under `Here.Sdk.Common.Map`:
- `public enum MapScheme { NormalDay, NormalNight, SatelliteDay, HybridDay, HybridNight, LiteDay, LiteNight, LiteHybridDay }`.
- `public enum LineCap { Butt, Round, Square }`.
- `public enum VisibilityState { Visible, Hidden }`.
- `public enum MapProjection { Globe, WebMercator }`.

#### Scenario: MapScheme.NormalDay is the default scheme
- **WHEN** `default(MapScheme)` is evaluated
- **THEN** the value equals `MapScheme.NormalDay`
