using System;
using System.Collections.Generic;

namespace Here.Sdk.Premium.Common.Geography;

/// <summary>Immutable ordered sequence of <see cref="GeoCoordinates"/> vertices.</summary>
public sealed record GeoPolyline
{
    /// <summary>An empty polyline with no vertices.</summary>
    public static readonly GeoPolyline Empty = new(Array.Empty<GeoCoordinates>());

    /// <summary>Ordered vertex sequence.</summary>
    public IReadOnlyList<GeoCoordinates> Vertices { get; }

    /// <summary>Initializes a new <see cref="GeoPolyline"/>.</summary>
    public GeoPolyline(IReadOnlyList<GeoCoordinates> vertices)
    {
        Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
    }

    /// <summary>
    /// Computes the total geodetic length in meters by summing Haversine distances
    /// between consecutive vertices.
    /// </summary>
    public double Length()
    {
        if (Vertices.Count < 2) return 0.0;

        double total = 0.0;
        for (int i = 1; i < Vertices.Count; i++)
            total += HaversineDistance(Vertices[i - 1], Vertices[i]);

        return total;
    }

    private static double HaversineDistance(GeoCoordinates a, GeoCoordinates b)
    {
        const double r = EarthConstants.MeanRadiusMeters;
        double dLat = ToRadians(b.Latitude - a.Latitude);
        double dLon = ToRadians(b.Longitude - a.Longitude);
        double sinDLat = Math.Sin(dLat / 2);
        double sinDLon = Math.Sin(dLon / 2);
        double h = sinDLat * sinDLat
                   + Math.Cos(ToRadians(a.Latitude)) * Math.Cos(ToRadians(b.Latitude))
                   * sinDLon * sinDLon;
        return 2.0 * r * Math.Asin(Math.Sqrt(h));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
