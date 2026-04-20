using System;
using FluentAssertions;
using Here.Sdk.Premium.Common.Geography;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Geography;

public sealed class GeoCoordinatesTests
{
    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(-90.0, -180.0)]
    [InlineData(90.0, 180.0)]
    [InlineData(48.8566, 2.3522)]
    public void Constructor_ValidCoordinates_StoresValues(double lat, double lon)
    {
        var coords = new GeoCoordinates(lat, lon);
        coords.Latitude.Should().Be(lat);
        coords.Longitude.Should().Be(lon);
        coords.Altitude.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithAltitude_StoresAltitude()
    {
        var coords = new GeoCoordinates(48.0, 11.0, 520.5);
        coords.Altitude.Should().Be(520.5);
    }

    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void Constructor_LatitudeOutOfBounds_ThrowsArgumentOutOfRangeException(double lat)
    {
        var act = () => new GeoCoordinates(lat, 0.0);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("latitude");
    }

    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    public void Constructor_LongitudeOutOfBounds_ThrowsArgumentOutOfRangeException(double lon)
    {
        var act = () => new GeoCoordinates(0.0, lon);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("longitude");
    }

    [Fact]
    public void ToString_UsesInvariantCulture_ReturnsExpectedFormat()
    {
        var coords = new GeoCoordinates(48.8566, 2.3522);
        coords.ToString().Should().Be("(48.8566, 2.3522)");
    }

    [Fact]
    public void ToString_WithAltitude_IncludesAltitude()
    {
        var coords = new GeoCoordinates(48.0, 11.0, 520.0);
        coords.ToString().Should().Be("(48, 11, 520)");
    }

    [Fact]
    public void Equality_SameValues_AreEqual()
    {
        var a = new GeoCoordinates(48.0, 11.0, 500.0);
        var b = new GeoCoordinates(48.0, 11.0, 500.0);
        a.Should().Be(b);
    }
}
