using FluentAssertions;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Units;

public sealed class SpeedTests
{
    [Fact]
    public void ToKph_10MetersPerSecond_Returns36Kph()
    {
        var speed = new Speed(10.0);
        speed.ToKph().Should().BeApproximately(36.0, 1e-9);
    }

    [Fact]
    public void FromKph_RoundTrip()
    {
        var speed = Speed.FromKph(100.0);
        speed.ToKph().Should().BeApproximately(100.0, 1e-9);
    }

    [Fact]
    public void FromMph_RoundTrip()
    {
        var speed = Speed.FromMph(60.0);
        speed.ToMph().Should().BeApproximately(60.0, 1e-9);
    }

    [Fact]
    public void Zero_HasZeroMetersPerSecond()
    {
        Speed.Zero.MetersPerSecond.Should().Be(0.0);
    }

    [Fact]
    public void ToString_InvariantFormat()
    {
        new Speed(5).ToString().Should().Be("5 m/s");
    }
}
