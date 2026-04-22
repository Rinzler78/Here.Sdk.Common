using FluentAssertions;
using Here.Sdk.Common.Geography;
using Xunit;

namespace Here.Sdk.Common.IntegrationTests.Geography;

/// <summary>
/// Integration tests for <see cref="FlexiblePolyline"/> encoding and <see cref="GeoPolyline"/> composition.
/// These tests exercise the full encode → decode → length pipeline across modules.
/// </summary>
public sealed class FlexiblePolylineRoundtripTests
{
    [Fact]
    public void Encode_ThenDecode_PreservesVertices()
    {
        var original = new[]
        {
            new GeoCoordinates(48.8566, 2.3522),
            new GeoCoordinates(51.5074, -0.1278),
            new GeoCoordinates(52.5200, 13.4050),
        };

        string encoded = FlexiblePolyline.Encode(original);
        GeoPolyline decoded = FlexiblePolyline.Decode(encoded);

        decoded.Vertices.Should().HaveCount(original.Length);
        for (int i = 0; i < original.Length; i++)
        {
            decoded.Vertices[i].Latitude.Should().BeApproximately(original[i].Latitude, 1e-4);
            decoded.Vertices[i].Longitude.Should().BeApproximately(original[i].Longitude, 1e-4);
        }
    }

    [Fact]
    public void Decode_ThenLength_ReturnsPositiveDistance()
    {
        var coords = new[]
        {
            new GeoCoordinates(48.8566, 2.3522),
            new GeoCoordinates(51.5074, -0.1278),
        };

        string encoded = FlexiblePolyline.Encode(coords);
        GeoPolyline polyline = FlexiblePolyline.Decode(encoded);

        polyline.Length().Should().BeGreaterThan(0);
    }

    [Fact]
    public void Encode_ThenDecode_EmptySequence_ProducesEmptyPolyline()
    {
        string encoded = FlexiblePolyline.Encode(Array.Empty<GeoCoordinates>());
        GeoPolyline decoded = FlexiblePolyline.Decode(encoded);

        decoded.Vertices.Should().BeEmpty();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(7)]
    public void Roundtrip_AtPrecision_MaintainsAccuracy(byte precision)
    {
        var coords = new[]
        {
            new GeoCoordinates(40.7128, -74.0060),
            new GeoCoordinates(34.0522, -118.2437),
        };

        double tolerance = Math.Pow(10, -precision + 1);
        string encoded = FlexiblePolyline.Encode(coords, precision);
        GeoPolyline decoded = FlexiblePolyline.Decode(encoded);

        for (int i = 0; i < coords.Length; i++)
        {
            decoded.Vertices[i].Latitude.Should().BeApproximately(coords[i].Latitude, tolerance);
            decoded.Vertices[i].Longitude.Should().BeApproximately(coords[i].Longitude, tolerance);
        }
    }

    [Fact]
    public void GeoPolyline_Length_MatchesHaversineForKnownSegment()
    {
        // Paris → London: approx 340 km
        var coords = new[]
        {
            new GeoCoordinates(48.8566, 2.3522),
            new GeoCoordinates(51.5074, -0.1278),
        };

        string encoded = FlexiblePolyline.Encode(coords);
        GeoPolyline polyline = FlexiblePolyline.Decode(encoded);

        polyline.Length().Should().BeInRange(330_000, 360_000);
    }
}
