using System;
using System.Collections.Generic;
using FluentAssertions;
using Here.Sdk.Common.Errors;
using Here.Sdk.Common.Geography;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Geography;

public sealed class FlexiblePolylineTests
{
    [Fact]
    public void EncodeDecode_RoundTrip_PreservesVerticesWithinPrecision()
    {
        var vertices = new List<GeoCoordinates>
        {
            new(50.1022829, 8.6982122),
            new(50.1020076, 8.6956695),
            new(50.1006313, 8.6914960),
        };

        var encoded = FlexiblePolyline.Encode(vertices);
        var decoded = FlexiblePolyline.Decode(encoded);

        decoded.Vertices.Should().HaveCount(vertices.Count);
        for (int i = 0; i < vertices.Count; i++)
        {
            decoded.Vertices[i].Latitude.Should().BeApproximately(vertices[i].Latitude, 1e-5);
            decoded.Vertices[i].Longitude.Should().BeApproximately(vertices[i].Longitude, 1e-5);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Decode_NullOrEmpty_ThrowsHereInvalidRequestException(string? input)
    {
        var act = () => FlexiblePolyline.Decode(input!);
        act.Should().Throw<HereInvalidRequestException>();
    }

    [Fact]
    public void Decode_InvalidInput_ThrowsHereInvalidRequestException()
    {
        var act = () => FlexiblePolyline.Decode("!!!invalid!!!");
        act.Should().Throw<HereInvalidRequestException>();
    }

    [Fact]
    public void Encode_EmptySequence_ReturnsDecodableEmptyPolyline()
    {
        var encoded = FlexiblePolyline.Encode([]);
        var decoded = FlexiblePolyline.Decode(encoded);
        decoded.Vertices.Should().BeEmpty();
    }

    [Fact]
    public void Encode_NullCoordinates_Throws()
    {
        var act = () => FlexiblePolyline.Encode(null!);
        act.Should().Throw<ArgumentNullException>().WithParameterName("coordinates");
    }

    [Fact]
    public void Encode_PrecisionBelowMinimum_Throws()
    {
        var act = () => FlexiblePolyline.Encode([], precision: 0);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("precision");
    }

    [Fact]
    public void Encode_PrecisionAboveMaximum_Throws()
    {
        var act = () => FlexiblePolyline.Encode([], precision: 16);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("precision");
    }

    [Fact]
    public void Encode_HighPrecision_RoundTrips()
    {
        var vertices = new List<GeoCoordinates> { new(48.8566, 2.3522) };
        var encoded = FlexiblePolyline.Encode(vertices, precision: 7);
        var decoded = FlexiblePolyline.Decode(encoded);
        decoded.Vertices[0].Latitude.Should().BeApproximately(48.8566, 1e-7);
    }
}
