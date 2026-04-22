using FluentAssertions;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.IntegrationTests.Units;

/// <summary>
/// Integration tests verifying interoperability between <see cref="Distance"/>,
/// <see cref="Speed"/>, and <see cref="Duration"/> value objects.
/// </summary>
public sealed class DistanceSpeedDurationTests
{
    [Fact]
    public void Distance_FromKilometers_RoundTrip()
    {
        Distance d = Distance.FromKilometers(42.195);

        d.ToKilometers().Should().BeApproximately(42.195, 1e-9);
        d.Meters.Should().BeApproximately(42_195.0, 1e-6);
    }

    [Fact]
    public void Speed_FromKph_AndDistance_ComputesTravelTime()
    {
        Distance distance = Distance.FromKilometers(100);
        Speed speed = Speed.FromKph(120);

        double hours = distance.ToKilometers() / speed.ToKph();
        Duration duration = Duration.FromMinutes(hours * 60);

        duration.TotalSeconds.Should().BeApproximately(3000, 0.1);
    }

    [Fact]
    public void Distance_Addition_IsCommutative()
    {
        Distance a = Distance.FromKilometers(10);
        Distance b = Distance.FromMiles(5);

        (a + b).Meters.Should().BeApproximately((b + a).Meters, 1e-9);
    }

    [Fact]
    public void Duration_Addition_Preserves_TotalSeconds()
    {
        Duration leg1 = Duration.FromMinutes(30);
        Duration leg2 = Duration.FromMinutes(45);

        Duration total = leg1 + leg2;

        total.TotalSeconds.Should().BeApproximately(4500, 0.001);
    }

    [Fact]
    public void Speed_FromMph_RoundTrip()
    {
        Speed s = Speed.FromMph(60);

        s.ToMph().Should().BeApproximately(60, 1e-6);
    }

    [Fact]
    public void Distance_Scalar_Multiplication_IsAssociative()
    {
        Distance d = Distance.FromKilometers(5);

        (d * 3.0).Meters.Should().BeApproximately((3.0 * d).Meters, 1e-9);
    }
}
