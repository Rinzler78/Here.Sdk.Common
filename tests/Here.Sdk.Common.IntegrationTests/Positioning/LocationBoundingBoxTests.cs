using FluentAssertions;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Positioning;
using Xunit;

namespace Here.Sdk.Common.IntegrationTests.Positioning;

/// <summary>
/// Integration tests verifying <see cref="Location"/> coordinates interact
/// correctly with <see cref="GeoBoundingBox.Contains"/>.
/// </summary>
public sealed class LocationBoundingBoxTests
{
    private static readonly GeoBoundingBox EuropeBbox = new(
        new GeoCoordinates(35.0, -10.0),
        new GeoCoordinates(71.0, 40.0));

    [Fact]
    public void Location_InEurope_IsContainedInEuropeBbox()
    {
        var paris = new Location(new GeoCoordinates(48.8566, 2.3522), DateTimeOffset.UtcNow);

        EuropeBbox.Contains(paris.Coordinates).Should().BeTrue();
    }

    [Fact]
    public void Location_OutsideEurope_IsNotContainedInEuropeBbox()
    {
        var newYork = new Location(new GeoCoordinates(40.7128, -74.0060), DateTimeOffset.UtcNow);

        EuropeBbox.Contains(newYork.Coordinates).Should().BeFalse();
    }

    [Fact]
    public void Location_WithOptionalFields_PreservesCoordinates()
    {
        var coords = new GeoCoordinates(52.5200, 13.4050);
        var loc = new Location(coords, DateTimeOffset.UtcNow)
        {
            BearingInDegrees = 90.0,
            SpeedInMetersPerSecond = 14.0,
        };

        loc.Coordinates.Latitude.Should().Be(coords.Latitude);
        loc.Coordinates.Longitude.Should().Be(coords.Longitude);
        loc.BearingInDegrees.Should().Be(90.0);
        loc.SpeedInMetersPerSecond.Should().Be(14.0);
    }

    [Fact]
    public void MultipleLocations_AllInsideBbox_ContainedCorrectly()
    {
        var locations = new[]
        {
            new Location(new GeoCoordinates(48.8566, 2.3522), DateTimeOffset.UtcNow), // Paris
            new Location(new GeoCoordinates(51.5074, -0.1278), DateTimeOffset.UtcNow), // London
            new Location(new GeoCoordinates(52.5200, 13.4050), DateTimeOffset.UtcNow), // Berlin
        };

        locations.Should().AllSatisfy(loc =>
            EuropeBbox.Contains(loc.Coordinates).Should().BeTrue());
    }
}
