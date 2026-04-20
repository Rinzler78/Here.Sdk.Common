using FluentAssertions;
using Here.Sdk.Premium.Common.Common;
using Here.Sdk.Premium.Common.Ev;
using Here.Sdk.Premium.Common.Map;
using Here.Sdk.Premium.Common.Navigation;
using Here.Sdk.Premium.Common.Positioning;
using Here.Sdk.Premium.Common.Routing;
using Here.Sdk.Premium.Common.Search;
using Here.Sdk.Premium.Common.Traffic;
using Here.Sdk.Premium.Common.Transport;
using Here.Sdk.Premium.Common.Units;
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

    [Fact]
    public void UnitSystem_Metric_IsOrdinal0() =>
        ((int)UnitSystem.Metric).Should().Be(0);

    [Fact]
    public void DayOfWeek_Monday_IsOrdinal0() =>
        ((int)Search.DayOfWeek.Monday).Should().Be(0);

    [Fact]
    public void LocationAccuracy_Default_IsNavigationAccuracy() =>
        default(LocationAccuracy).Should().Be(LocationAccuracy.NavigationAccuracy);

    [Fact]
    public void EvseState_Default_IsNotUnknown() =>
        default(EvseState).Should().NotBe(EvseState.Unknown);

    [Fact]
    public void MapScheme_Default_IsNormalDay() =>
        default(MapScheme).Should().Be(MapScheme.NormalDay);

    [Fact]
    public void SpeedWarningStatus_HasExactlyTwoMembers()
    {
        var members = Enum.GetValues<SpeedWarningStatus>();
        members.Should().HaveCount(2);
        members.Should().Contain(SpeedWarningStatus.SpeedLimitExceeded);
        members.Should().Contain(SpeedWarningStatus.SpeedLimitRestored);
    }

    [Fact]
    public void ChargingConnectorType_SaeJ3400_IsPresent() =>
        Enum.IsDefined(ChargingConnectorType.SaeJ3400).Should().BeTrue();
}
