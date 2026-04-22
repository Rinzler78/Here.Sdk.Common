using FluentAssertions;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.E2ETests.Scenarios;

/// <summary>
/// End-to-end scenario: a consumer models a route as a GeoPolyline encoded
/// via FlexiblePolyline, then computes distance and estimated travel time.
/// No external I/O; validates the full value-object pipeline behaves like
/// a real SDK consumer would use it.
/// </summary>
public sealed class RoutePlanningScenarioTests
{
    [Fact]
    public void Consumer_CanEncode_RoutePolyline_AndComputeDistanceAndETA()
    {
        // Arrange: Paris → Brussels → Amsterdam
        var waypoints = new[]
        {
            new GeoCoordinates(48.8566, 2.3522),
            new GeoCoordinates(50.8503, 4.3517),
            new GeoCoordinates(52.3702, 4.8952),
        };

        // Act: encode and decode (simulates storing and retrieving the route)
        string encoded = FlexiblePolyline.Encode(waypoints);
        GeoPolyline route = FlexiblePolyline.Decode(encoded);

        // Compute total distance and ETA at 100 km/h
        double totalMeters = route.Length();
        Distance distance = new Distance(totalMeters);
        Speed cruiseSpeed = Speed.FromKph(100);
        double travelHours = distance.ToKilometers() / cruiseSpeed.ToKph();
        Duration eta = Duration.FromMinutes(travelHours * 60);

        // Assert
        route.Vertices.Should().HaveCount(3);
        distance.Meters.Should().BeGreaterThan(400_000); // at least 400 km
        eta.TotalSeconds.Should().BeGreaterThan(3600);    // more than 1 hour
    }

    [Fact]
    public void Consumer_CanSplit_RouteIntoLegs_AndSumDistances()
    {
        var points = new[]
        {
            new GeoCoordinates(48.8566, 2.3522),
            new GeoCoordinates(50.8503, 4.3517),
            new GeoCoordinates(52.3702, 4.8952),
        };

        // Simulate two legs
        var leg1 = new GeoPolyline(new[] { points[0], points[1] });
        var leg2 = new GeoPolyline(new[] { points[1], points[2] });
        var full = new GeoPolyline(points);

        Distance legTotal = new Distance(leg1.Length()) + new Distance(leg2.Length());
        Distance fullDist = new Distance(full.Length());

        legTotal.Meters.Should().BeApproximately(fullDist.Meters, 0.1);
    }
}
