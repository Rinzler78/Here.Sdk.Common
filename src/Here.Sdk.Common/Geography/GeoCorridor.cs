using System;

namespace Here.Sdk.Common.Geography;

/// <summary>Corridor along a polyline with a fixed half-width on each side.</summary>
public sealed record GeoCorridor
{
    /// <summary>Center-line polyline.</summary>
    public GeoPolyline Polyline { get; }

    /// <summary>Half-width in meters from the center-line. Must be &gt;= 0.</summary>
    public int HalfWidthInMeters { get; }

    /// <summary>Initializes a new <see cref="GeoCorridor"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="halfWidthInMeters"/> is negative.</exception>
    public GeoCorridor(GeoPolyline polyline, int halfWidthInMeters)
    {
        if (halfWidthInMeters < 0)
            throw new ArgumentOutOfRangeException(nameof(halfWidthInMeters), "HalfWidthInMeters must be >= 0.");
        Polyline = polyline ?? throw new ArgumentNullException(nameof(polyline));
        HalfWidthInMeters = halfWidthInMeters;
    }
}
