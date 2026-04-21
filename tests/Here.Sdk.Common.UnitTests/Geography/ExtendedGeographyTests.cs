using System;
using System.Collections.Generic;
using FluentAssertions;
using Here.Sdk.Common.Geography;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Geography;

public sealed class GeoCircleTests
{
    [Fact]
    public void Constructor_ValidRadius_Stores()
    {
        var center = new GeoCoordinates(0, 0);
        var circle = new GeoCircle(center, 500);
        circle.Center.Should().Be(center);
        circle.RadiusInMeters.Should().Be(500);
    }

    [Fact]
    public void Constructor_ZeroRadius_Allowed()
    {
        var act = () => new GeoCircle(new GeoCoordinates(0, 0), 0);
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_NegativeRadius_Throws()
    {
        var act = () => new GeoCircle(new GeoCoordinates(0, 0), -1);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("radiusInMeters");
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        var center = new GeoCoordinates(48, 2);
        var a = new GeoCircle(center, 100);
        var b = new GeoCircle(center, 100);
        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}

public sealed class GeoPolygonTests
{
    [Fact]
    public void Constructor_ThreeVertices_Succeeds()
    {
        var vertices = new List<GeoCoordinates>
        {
            new(0, 0), new(1, 0), new(0, 1)
        };
        var act = () => new GeoPolygon(vertices);
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_TwoVertices_Throws()
    {
        var vertices = new List<GeoCoordinates> { new(0, 0), new(1, 1) };
        var act = () => new GeoPolygon(vertices);
        act.Should().Throw<ArgumentException>().WithMessage("*at least 3 vertices*");
    }

    [Fact]
    public void Constructor_Null_Throws()
    {
        var act = () => new GeoPolygon(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Vertices_AreStored()
    {
        var vertices = new List<GeoCoordinates> { new(0, 0), new(1, 0), new(0, 1) };
        var polygon = new GeoPolygon(vertices);
        polygon.Vertices.Should().BeEquivalentTo(vertices);
    }
}

public sealed class GeoCorridorTests
{
    private static readonly GeoPolyline _line = new(
        new List<GeoCoordinates> { new(0, 0), new(1, 1) });

    [Fact]
    public void Constructor_ValidHalfWidth_Stores()
    {
        var corridor = new GeoCorridor(_line, 50);
        corridor.Polyline.Should().Be(_line);
        corridor.HalfWidthInMeters.Should().Be(50);
    }

    [Fact]
    public void Constructor_ZeroHalfWidth_Allowed()
    {
        var act = () => new GeoCorridor(_line, 0);
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_NegativeHalfWidth_Throws()
    {
        var act = () => new GeoCorridor(_line, -1);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("halfWidthInMeters");
    }

    [Fact]
    public void Constructor_NullPolyline_Throws()
    {
        var act = () => new GeoCorridor(null!, 10);
        act.Should().Throw<ArgumentNullException>();
    }
}

public sealed class GeoOrientationTests
{
    [Fact]
    public void Constructor_AllNull_Stores()
    {
        var o = new GeoOrientation();
        o.BearingInDegrees.Should().BeNull();
        o.TiltInDegrees.Should().BeNull();
        o.RollInDegrees.Should().BeNull();
    }

    [Fact]
    public void Bearing_NormalisedToZero360()
    {
        new GeoOrientation(bearingInDegrees: -90).BearingInDegrees.Should().BeApproximately(270, 1e-9);
        new GeoOrientation(bearingInDegrees: 450).BearingInDegrees.Should().BeApproximately(90, 1e-9);
    }

    [Fact]
    public void Tilt_ClampedTo90()
    {
        new GeoOrientation(tiltInDegrees: 100).TiltInDegrees.Should().Be(90);
        new GeoOrientation(tiltInDegrees: -100).TiltInDegrees.Should().Be(-90);
    }

    [Fact]
    public void Roll_ClampedTo180()
    {
        new GeoOrientation(rollInDegrees: 200).RollInDegrees.Should().Be(180);
        new GeoOrientation(rollInDegrees: -200).RollInDegrees.Should().Be(-180);
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        var a = new GeoOrientation(45, 10, 5);
        var b = new GeoOrientation(45, 10, 5);
        a.Should().Be(b);
    }
}
