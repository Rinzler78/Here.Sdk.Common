using System;

namespace Here.Sdk.Premium.Common.Geography;

/// <summary>Axis-aligned bounding box defined by south-west and north-east corners, with antimeridian support.</summary>
public readonly record struct GeoBoundingBox
{
    /// <summary>South-west corner.</summary>
    public GeoCoordinates SouthWest { get; }

    /// <summary>North-east corner.</summary>
    public GeoCoordinates NorthEast { get; }

    /// <summary>Initializes a new <see cref="GeoBoundingBox"/>.</summary>
    /// <exception cref="ArgumentException">When <c>SouthWest.Latitude &gt; NorthEast.Latitude</c>.</exception>
    public GeoBoundingBox(GeoCoordinates southWest, GeoCoordinates northEast)
    {
        if (southWest.Latitude > northEast.Latitude)
            throw new ArgumentException(
                $"SouthWest.Latitude ({southWest.Latitude}) must be <= NorthEast.Latitude ({northEast.Latitude}).",
                nameof(southWest));

        SouthWest = southWest;
        NorthEast = northEast;
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="point"/> is inside this bounding box.
    /// Handles antimeridian-crossing boxes (when <c>SouthWest.Longitude &gt; NorthEast.Longitude</c>).
    /// </summary>
    public bool Contains(GeoCoordinates point)
    {
        if (point.Latitude < SouthWest.Latitude || point.Latitude > NorthEast.Latitude)
            return false;

        if (SouthWest.Longitude > NorthEast.Longitude)
            return point.Longitude >= SouthWest.Longitude || point.Longitude <= NorthEast.Longitude;

        return point.Longitude >= SouthWest.Longitude && point.Longitude <= NorthEast.Longitude;
    }
}
