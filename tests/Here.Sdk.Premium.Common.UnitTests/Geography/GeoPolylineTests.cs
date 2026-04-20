using FluentAssertions;
using Here.Sdk.Premium.Common.Geography;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Geography;

public sealed class GeoPolylineTests
{
    [Fact]
    public void Empty_HasZeroVertices_ZeroLength()
    {
        GeoPolyline.Empty.Vertices.Should().BeEmpty();
        GeoPolyline.Empty.Length().Should().Be(0.0);
    }

    [Fact]
    public void Length_SingleVertex_ReturnsZero()
    {
        var poly = new GeoPolyline([new GeoCoordinates(48.0, 11.0)]);
        poly.Length().Should().Be(0.0);
    }

    [Fact]
    public void Length_TwoVerticesSamePoint_ReturnsZero()
    {
        var c = new GeoCoordinates(48.0, 11.0);
        var poly = new GeoPolyline([c, c]);
        poly.Length().Should().Be(0.0);
    }

    [Fact]
    public void Length_MultiVertexPolyline_SumsHaversineSegments()
    {
        // Paris to Berlin approx 878 km
        var paris = new GeoCoordinates(48.8566, 2.3522);
        var berlin = new GeoCoordinates(52.5200, 13.4050);
        var poly = new GeoPolyline([paris, berlin]);
        poly.Length().Should().BeInRange(870_000, 890_000);
    }
}
