using System;
using FluentAssertions;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Positioning;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Positioning;

public sealed class LocationTests
{
    private static readonly GeoCoordinates _coords = new(48.8566, 2.3522);
    private static readonly DateTimeOffset _ts = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Constructor_MinimalFields_NullableFieldsAreNull()
    {
        var loc = new Location(_coords, _ts);
        loc.Coordinates.Should().Be(_coords);
        loc.Timestamp.Should().Be(_ts);
        loc.BearingInDegrees.Should().BeNull();
        loc.SpeedInMetersPerSecond.Should().BeNull();
        loc.HorizontalAccuracyInMeters.Should().BeNull();
        loc.VerticalAccuracyInMeters.Should().BeNull();
        loc.BearingAccuracyInDegrees.Should().BeNull();
        loc.SpeedAccuracyInMetersPerSecond.Should().BeNull();
        loc.AltitudeAccuracyInMeters.Should().BeNull();
    }

    [Fact]
    public void InitProperties_FullConstruction_Stored()
    {
        var loc = new Location(_coords, _ts)
        {
            BearingInDegrees = 90,
            SpeedInMetersPerSecond = 10,
            HorizontalAccuracyInMeters = 5,
            VerticalAccuracyInMeters = 3,
            BearingAccuracyInDegrees = 2,
            SpeedAccuracyInMetersPerSecond = 0.5,
            AltitudeAccuracyInMeters = 4
        };
        loc.BearingInDegrees.Should().Be(90);
        loc.AltitudeAccuracyInMeters.Should().Be(4);
    }

    [Fact]
    public void Equality_IdenticalValues_Equal()
    {
        var a = new Location(_coords, _ts) { BearingInDegrees = 45 };
        var b = new Location(_coords, _ts) { BearingInDegrees = 45 };
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentBearing_NotEqual()
    {
        var a = new Location(_coords, _ts) { BearingInDegrees = 45 };
        var b = new Location(_coords, _ts) { BearingInDegrees = 90 };
        a.Should().NotBe(b);
    }
}
