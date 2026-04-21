using FluentAssertions;
using Here.Sdk.Common.Geography;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Geography;

public sealed class GeoBearingTests
{
    [Theory]
    [InlineData(0.0, 0.0)]
    [InlineData(90.0, 90.0)]
    [InlineData(359.9, 359.9)]
    [InlineData(360.0, 0.0)]
    [InlineData(720.0, 0.0)]
    [InlineData(450.0, 90.0)]
    public void Constructor_NormalizesToZeroTo360(double input, double expected)
    {
        var bearing = new GeoBearing(input);
        bearing.DegreesFromNorth.Should().BeApproximately(expected, 1e-9);
    }

    [Theory]
    [InlineData(-90.0, 270.0)]
    [InlineData(-180.0, 180.0)]
    [InlineData(-1.0, 359.0)]
    public void Constructor_NegativeValues_WrapPositive(double input, double expected)
    {
        var bearing = new GeoBearing(input);
        bearing.DegreesFromNorth.Should().BeApproximately(expected, 1e-9);
    }

    [Fact]
    public void Equality_EquivalentInputs_AreEqual()
    {
        var a = new GeoBearing(360.0);
        var b = new GeoBearing(0.0);
        a.Should().Be(b);
    }

    [Fact]
    public void ToString_ReturnsDegreesSymbol()
    {
        var bearing = new GeoBearing(90.0);
        bearing.ToString().Should().EndWith("\u00b0");
    }
}
