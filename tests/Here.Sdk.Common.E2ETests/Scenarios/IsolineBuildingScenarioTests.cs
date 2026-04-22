using FluentAssertions;
using Here.Sdk.Common.Geography;
using Here.Sdk.Common.Units;
using Xunit;

namespace Here.Sdk.Common.E2ETests.Scenarios;

/// <summary>
/// End-to-end scenario: a consumer builds an isoline (reachability polygon)
/// approximation using value objects, then checks point-in-bounding-box
/// as a surrogate for isoline containment.
/// </summary>
public sealed class IsolineBuildingScenarioTests
{
    [Fact]
    public void Consumer_CanApproximate_IsolineAsBoundingBox_FromRangeDistance()
    {
        var origin = new GeoCoordinates(48.8566, 2.3522); // Paris
        Distance range = Distance.FromKilometers(50);

        // Approximate bounding box: 0.5° ≈ ~55 km at mid-latitudes
        double degDelta = range.ToKilometers() / 111.0;
        var isolineBbox = new GeoBoundingBox(
            new GeoCoordinates(origin.Latitude - degDelta, origin.Longitude - degDelta),
            new GeoCoordinates(origin.Latitude + degDelta, origin.Longitude + degDelta));

        // Origin itself must be inside
        isolineBbox.Contains(origin).Should().BeTrue();

        // A point 80 km away must be outside
        var farPoint = new GeoCoordinates(origin.Latitude + 0.75, origin.Longitude);
        isolineBbox.Contains(farPoint).Should().BeFalse();
    }

    [Fact]
    public void Consumer_CanEncode_IsolineBoundaryAsPolyline()
    {
        // Simulate 8-point approximation of a circular isoline
        var center = new GeoCoordinates(52.5200, 13.4050); // Berlin
        double radiusDeg = 0.25;
        int steps = 8;

        var boundary = Enumerable.Range(0, steps)
            .Select(i =>
            {
                double angle = 2 * Math.PI * i / steps;
                return new GeoCoordinates(
                    center.Latitude + radiusDeg * Math.Cos(angle),
                    center.Longitude + radiusDeg * Math.Sin(angle));
            })
            .ToArray();

        string encoded = FlexiblePolyline.Encode(boundary);
        GeoPolyline decoded = FlexiblePolyline.Decode(encoded);

        decoded.Vertices.Should().HaveCount(steps);
        decoded.Length().Should().BeGreaterThan(0);
    }
}
