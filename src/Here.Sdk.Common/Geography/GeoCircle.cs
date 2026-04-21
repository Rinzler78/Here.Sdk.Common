using System;

namespace Here.Sdk.Common.Geography;

/// <summary>Circle defined by a center coordinate and a radius.</summary>
public readonly record struct GeoCircle
{
    /// <summary>Center of the circle.</summary>
    public GeoCoordinates Center { get; }

    /// <summary>Radius in meters. Must be &gt;= 0.</summary>
    public double RadiusInMeters { get; }

    /// <summary>Initializes a new <see cref="GeoCircle"/> and validates the radius.</summary>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="radiusInMeters"/> is negative.</exception>
    public GeoCircle(GeoCoordinates center, double radiusInMeters)
    {
        if (radiusInMeters < 0)
            throw new ArgumentOutOfRangeException(nameof(radiusInMeters), "Radius must be >= 0.");
        Center = center;
        RadiusInMeters = radiusInMeters;
    }
}
