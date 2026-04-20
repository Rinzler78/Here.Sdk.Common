using System;
using FluentAssertions;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.UnitTests.Units;

public sealed class DurationTests
{
    [Fact]
    public void Zero_IsZeroTimeSpan()
    {
        Duration.Zero.Value.Should().Be(TimeSpan.Zero);
        Duration.Zero.TotalSeconds.Should().Be(0);
    }

    [Fact]
    public void FromSeconds_RoundTrip()
    {
        Duration.FromSeconds(90).TotalSeconds.Should().BeApproximately(90, 1e-9);
    }

    [Fact]
    public void FromMinutes_RoundTrip()
    {
        Duration.FromMinutes(2).TotalSeconds.Should().BeApproximately(120, 1e-9);
    }

    [Fact]
    public void Addition_Composes()
    {
        var result = Duration.FromSeconds(30) + Duration.FromSeconds(45);
        result.TotalSeconds.Should().BeApproximately(75, 1e-9);
    }

    [Fact]
    public void Subtraction_Composes()
    {
        var result = Duration.FromSeconds(60) - Duration.FromSeconds(20);
        result.TotalSeconds.Should().BeApproximately(40, 1e-9);
    }

    [Fact]
    public void Equality_SameValue_Equal()
    {
        Duration.FromSeconds(10).Should().Be(Duration.FromSeconds(10));
    }

    [Fact]
    public void Equality_DifferentValue_NotEqual()
    {
        Duration.FromSeconds(10).Should().NotBe(Duration.FromSeconds(20));
    }

    [Fact]
    public void ToString_InvariantFormat()
    {
        Duration.FromSeconds(90).ToString().Should().Be("00:01:30");
    }
}
