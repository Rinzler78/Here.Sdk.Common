using FluentAssertions;
using Here.Sdk.Premium.Common.Common;
using Here.Sdk.Premium.Common.Ev;
using Here.Sdk.Premium.Common.Map;
using Here.Sdk.Premium.Common.Positioning;
using Here.Sdk.Premium.Common.Traffic;
using Here.Sdk.Premium.Common.Transport;
using Xunit;

namespace Here.Sdk.Premium.Common.UnitTests.Enums;

public sealed class EnumOrdinalStabilityTests
{
    [Fact]
    public void TransportMode_Car_IsOrdinal0() =>
        ((int)TransportMode.Car).Should().Be(0);

    [Fact]
    public void CardinalDirection_North_IsOrdinal0() =>
        ((int)CardinalDirection.North).Should().Be(0);

    [Fact]
    public void LocationAccuracy_NavigationAccuracy_IsOrdinal0() =>
        ((int)LocationAccuracy.NavigationAccuracy).Should().Be(0);

    [Fact]
    public void TrafficIncidentImpact_OrderedBySeverity()
    {
        ((int)TrafficIncidentImpact.Unknown).Should().BeLessThan((int)TrafficIncidentImpact.LowImpact);
        ((int)TrafficIncidentImpact.LowImpact).Should().BeLessThan((int)TrafficIncidentImpact.Minor);
        ((int)TrafficIncidentImpact.Minor).Should().BeLessThan((int)TrafficIncidentImpact.Major);
        ((int)TrafficIncidentImpact.Major).Should().BeLessThan((int)TrafficIncidentImpact.Critical);
    }

    [Fact]
    public void HazardousMaterial_FlagsArePowersOfTwo()
    {
        ((int)HazardousMaterial.Explosive).Should().Be(1);
        ((int)HazardousMaterial.Gas).Should().Be(2);
        ((int)HazardousMaterial.Flammable).Should().Be(4);
        (HazardousMaterial.Explosive | HazardousMaterial.Gas)
            .Should().HaveFlag(HazardousMaterial.Explosive)
            .And.HaveFlag(HazardousMaterial.Gas);
    }

    [Fact]
    public void EvseState_Unknown_IsNotOrdinal0()
    {
        // By design: ordinal 0 is Available (safe default), Unknown is 8
        ((int)EvseState.Available).Should().Be(0);
        ((int)EvseState.Unknown).Should().NotBe(0);
    }

    [Fact]
    public void MapScheme_NormalDay_IsOrdinal0() =>
        ((int)MapScheme.NormalDay).Should().Be(0);
}
