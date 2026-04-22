using FluentAssertions;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Positioning;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.E2ETests.Scenarios;

/// <summary>
/// End-to-end scenario: a consumer tracks a sequence of <see cref="Location"/>
/// updates and verifies containment inside a geofence <see cref="GeoBoundingBox"/>.
/// </summary>
public sealed class LocationTrackingScenarioTests
{
    [Fact]
    public void Consumer_CanDetect_LocationExitsGeofence()
    {
        var geofence = new GeoBoundingBox(
            new GeoCoordinates(48.80, 2.25),
            new GeoCoordinates(48.92, 2.45));

        var track = new[]
        {
            new Location(new GeoCoordinates(48.855, 2.340), DateTimeOffset.UtcNow),
            new Location(new GeoCoordinates(48.870, 2.370), DateTimeOffset.UtcNow.AddSeconds(30)),
            new Location(new GeoCoordinates(49.000, 2.500), DateTimeOffset.UtcNow.AddSeconds(60)), // exit
        };

        var inside = track.Where(l => geofence.Contains(l.Coordinates)).ToList();
        var outside = track.Where(l => !geofence.Contains(l.Coordinates)).ToList();

        inside.Should().HaveCount(2);
        outside.Should().HaveCount(1);
    }

    [Fact]
    public void Consumer_CanBuild_TrackPolyline_FromLocations()
    {
        var timestamps = Enumerable.Range(0, 5)
            .Select(i => DateTimeOffset.UtcNow.AddSeconds(i * 10))
            .ToArray();

        var locations = new[]
        {
            new Location(new GeoCoordinates(48.856, 2.352), timestamps[0]) { SpeedInMetersPerSecond = 14 },
            new Location(new GeoCoordinates(48.858, 2.355), timestamps[1]) { SpeedInMetersPerSecond = 15 },
            new Location(new GeoCoordinates(48.860, 2.358), timestamps[2]) { SpeedInMetersPerSecond = 13 },
            new Location(new GeoCoordinates(48.862, 2.361), timestamps[3]) { SpeedInMetersPerSecond = 14 },
            new Location(new GeoCoordinates(48.864, 2.364), timestamps[4]) { SpeedInMetersPerSecond = 16 },
        };

        var polyline = new GeoPolyline(locations.Select(l => l.Coordinates).ToArray());

        polyline.Vertices.Should().HaveCount(5);
        polyline.Length().Should().BeGreaterThan(0);

        double avgSpeed = locations.Average(l => l.SpeedInMetersPerSecond!.Value);
        Speed averageSpeed = new Speed(avgSpeed);
        averageSpeed.ToKph().Should().BeApproximately(51.84, 0.1);
    }
}
