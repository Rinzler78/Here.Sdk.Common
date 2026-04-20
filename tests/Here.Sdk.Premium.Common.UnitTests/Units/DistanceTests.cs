using FluentAssertions;
using Here.Sdk.Premium.Common.Units;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Units;

public sealed class DistanceTests
{
    [Fact]
    public void Zero_HasZeroMeters()
    {
        Distance.Zero.Meters.Should().Be(0.0);
    }

    [Fact]
    public void FromKilometers_ToKilometers_RoundTrip()
    {
        var d = Distance.FromKilometers(5.0);
        d.ToKilometers().Should().BeApproximately(5.0, 1e-10);
    }

    [Fact]
    public void FromMiles_ToMiles_RoundTrip()
    {
        var d = Distance.FromMiles(3.0);
        d.ToMiles().Should().BeApproximately(3.0, 1e-9);
    }

    [Fact]
    public void Addition_ReturnsSumOfMeters()
    {
        var result = new Distance(100) + new Distance(200);
        result.Meters.Should().Be(300);
    }

    [Fact]
    public void Subtraction_ReturnsDifferenceOfMeters()
    {
        var result = new Distance(500) - new Distance(200);
        result.Meters.Should().Be(300);
    }

    [Fact]
    public void Multiplication_ByScalar_Scales()
    {
        var result = new Distance(100) * 3.0;
        result.Meters.Should().Be(300);
    }
}
