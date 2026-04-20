using System;
using FluentAssertions;
using Here.Sdk.Premium.Common.Geography;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Geography;

public sealed class GeoBoundingBoxTests
{
    private static readonly GeoCoordinates Sw = new(47.0, 8.0);
    private static readonly GeoCoordinates Ne = new(49.0, 12.0);

    [Fact]
    public void Constructor_ValidCorners_Stores()
    {
        var box = new GeoBoundingBox(Sw, Ne);
        box.SouthWest.Should().Be(Sw);
        box.NorthEast.Should().Be(Ne);
    }

    [Fact]
    public void Constructor_SwLatitudeGreaterThanNeLatitude_Throws()
    {
        var act = () => new GeoBoundingBox(Ne, Sw);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Contains_InteriorPoint_ReturnsTrue()
    {
        var box = new GeoBoundingBox(Sw, Ne);
        box.Contains(new GeoCoordinates(48.0, 10.0)).Should().BeTrue();
    }

    [Fact]
    public void Contains_ExteriorPoint_ReturnsFalse()
    {
        var box = new GeoBoundingBox(Sw, Ne);
        box.Contains(new GeoCoordinates(50.0, 10.0)).Should().BeFalse();
    }

    [Fact]
    public void Contains_AntimeridianBox_HandlesWrap()
    {
        // Box crosses antimeridian: SW.Lon=170, NE.Lon=-170
        var sw = new GeoCoordinates(30.0, 170.0);
        var ne = new GeoCoordinates(40.0, -170.0);
        var box = new GeoBoundingBox(sw, ne);

        box.Contains(new GeoCoordinates(35.0, 175.0)).Should().BeTrue();
        box.Contains(new GeoCoordinates(35.0, -175.0)).Should().BeTrue();
        box.Contains(new GeoCoordinates(35.0, 0.0)).Should().BeFalse();
    }
}
