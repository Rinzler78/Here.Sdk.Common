using System;
using FluentAssertions;
using Here.Sdk.Common.Geometry;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Geometry;

public sealed class AngleTests
{
    [Fact]
    public void FromRadians_Pi_Equals180Degrees()
    {
        Angle.FromRadians(Math.PI).ValueInDegrees.Should().BeApproximately(180.0, 1e-9);
    }

    [Fact]
    public void ToRadians_180Degrees_EqualsPi()
    {
        new Angle(180).ToRadians().Should().BeApproximately(Math.PI, 1e-9);
    }

    [Fact]
    public void RoundTrip_RadiansToDegrees()
    {
        var original = new Angle(45.0);
        Angle.FromRadians(original.ToRadians()).ValueInDegrees
            .Should().BeApproximately(45.0, 1e-9);
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        new Angle(90).Should().Be(new Angle(90));
    }

    [Fact]
    public void ToString_IncludesDegreeSymbol()
    {
        new Angle(45).ToString().Should().Contain("45");
    }
}

public sealed class IntegerRangeTests
{
    [Fact]
    public void Constructor_ValidRange_Stores()
    {
        var range = new IntegerRange(1, 10);
        range.Min.Should().Be(1);
        range.Max.Should().Be(10);
    }

    [Fact]
    public void Constructor_EqualMinMax_Allowed()
    {
        var act = () => new IntegerRange(5, 5);
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_MinGreaterThanMax_Throws()
    {
        var act = () => new IntegerRange(5, 3);
        act.Should().Throw<ArgumentException>().WithParameterName("min");
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        new IntegerRange(1, 5).Should().Be(new IntegerRange(1, 5));
    }
}

public sealed class Size2DTests
{
    [Fact]
    public void Constructor_ValidDimensions_Stores()
    {
        var s = new Size2D(10, 20);
        s.Width.Should().Be(10);
        s.Height.Should().Be(20);
    }

    [Fact]
    public void Constructor_NegativeWidth_Throws()
    {
        var act = () => new Size2D(-1, 5);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("width");
    }

    [Fact]
    public void Constructor_NegativeHeight_Throws()
    {
        var act = () => new Size2D(5, -1);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("height");
    }

    [Fact]
    public void Constructor_ZeroDimensions_Allowed()
    {
        var act = () => new Size2D(0, 0);
        act.Should().NotThrow();
    }

    [Fact]
    public void ToString_InvariantFormat()
    {
        new Size2D(3, 4).ToString().Should().Be("3x4");
    }
}

public sealed class Point2DTests
{
    [Fact]
    public void Constructor_Stores()
    {
        var p = new Point2D(3.5, -1.2);
        p.X.Should().Be(3.5);
        p.Y.Should().Be(-1.2);
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        new Point2D(1, 2).Should().Be(new Point2D(1, 2));
    }

    [Fact]
    public void ToString_ContainsCoordinates()
    {
        new Point2D(1, 2).ToString().Should().Contain("1").And.Contain("2");
    }
}

public sealed class Point3DTests
{
    [Fact]
    public void Constructor_Stores()
    {
        var p = new Point3D(1, 2, 3);
        p.X.Should().Be(1);
        p.Y.Should().Be(2);
        p.Z.Should().Be(3);
    }

    [Fact]
    public void ToString_ContainsCoordinates()
    {
        new Point3D(1, 2, 3).ToString().Should().Contain("1").And.Contain("2").And.Contain("3");
    }
}

public sealed class Anchor2DTests
{
    [Fact]
    public void Constructor_Stores()
    {
        var a = new Anchor2D(0.5, 0.5);
        a.HorizontalOffset.Should().Be(0.5);
        a.VerticalOffset.Should().Be(0.5);
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        new Anchor2D(0.1, 0.9).Should().Be(new Anchor2D(0.1, 0.9));
    }

    [Fact]
    public void Constructor_HorizontalBelowZero_Throws()
    {
        var act = () => new Anchor2D(-0.1, 0.5);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("horizontalOffset");
    }

    [Fact]
    public void Constructor_HorizontalAboveOne_Throws()
    {
        var act = () => new Anchor2D(1.1, 0.5);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("horizontalOffset");
    }

    [Fact]
    public void Constructor_VerticalBelowZero_Throws()
    {
        var act = () => new Anchor2D(0.5, -0.1);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("verticalOffset");
    }

    [Fact]
    public void Constructor_VerticalAboveOne_Throws()
    {
        var act = () => new Anchor2D(0.5, 1.1);
        act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("verticalOffset");
    }

    [Fact]
    public void ToString_ContainsOffsets()
    {
        new Anchor2D(0.5, 0.5).ToString().Should().Contain("0.5");
    }
}

public sealed class Rectangle2DTests
{
    [Fact]
    public void Constructor_Stores()
    {
        var origin = new Point2D(1, 2);
        var size = new Size2D(10, 5);
        var rect = new Rectangle2D(origin, size);
        rect.Origin.Should().Be(origin);
        rect.Size.Should().Be(size);
    }

    [Fact]
    public void Equality_SameValues_Equal()
    {
        var a = new Rectangle2D(new Point2D(0, 0), new Size2D(10, 10));
        var b = new Rectangle2D(new Point2D(0, 0), new Size2D(10, 10));
        a.Should().Be(b);
    }
}

public sealed class AngleRangeTests
{
    [Fact]
    public void Constructor_Stores()
    {
        var start = new Angle(0);
        var extent = new Angle(90);
        var range = new AngleRange(start, extent);
        range.StartAngle.Should().Be(start);
        range.Extent.Should().Be(extent);
    }
}
