using System;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Positioning;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Positioning;

public sealed class LocationSnapshotTests
{
    private static readonly GeoCoordinates Coords = new(48.8566, 2.3522);
    private static readonly DateTimeOffset Ts = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public Task Snapshot_MinimalLocation() =>
        Verify(new Location(Coords, Ts));

    [Fact]
    public Task Snapshot_FullLocation() =>
        Verify(new Location(Coords, Ts)
        {
            BearingInDegrees = 90.0,
            SpeedInMetersPerSecond = 13.9,
            HorizontalAccuracyInMeters = 5.0,
            VerticalAccuracyInMeters = 3.0,
            BearingAccuracyInDegrees = 2.0,
            SpeedAccuracyInMetersPerSecond = 0.5,
            AltitudeAccuracyInMeters = 4.0,
        });
}
