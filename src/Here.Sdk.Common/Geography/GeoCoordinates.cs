using System;
using System.Globalization;

namespace Here.Sdk.Common.Geography;

/// <summary>Immutable WGS84 geographic coordinate pair with optional altitude.</summary>
public readonly record struct GeoCoordinates
{
    /// <summary>Latitude in degrees. Must be in [-90, 90].</summary>
    public double Latitude { get; }

    /// <summary>Longitude in degrees. Must be in [-180, 180].</summary>
    public double Longitude { get; }

    /// <summary>Altitude in meters above WGS84 ellipsoid, or <c>null</c> if unknown.</summary>
    public double? Altitude { get; }

    /// <summary>Initializes a new <see cref="GeoCoordinates"/> and validates WGS84 bounds.</summary>
    /// <exception cref="ArgumentOutOfRangeException">When latitude or longitude are out of bounds.</exception>
    public GeoCoordinates(double latitude, double longitude, double? altitude = null)
    {
        if (double.IsNaN(latitude) || double.IsInfinity(latitude) || latitude < -90.0 || latitude > 90.0)
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be in [-90, 90].");
        if (double.IsNaN(longitude) || double.IsInfinity(longitude) || longitude < -180.0 || longitude > 180.0)
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be in [-180, 180].");

        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
    }

    /// <summary>Returns a culture-invariant string representation.</summary>
    public override string ToString()
    {
        var alt = Altitude.HasValue
            ? string.Format(CultureInfo.InvariantCulture, ", {0}", Altitude.Value)
            : string.Empty;
        return string.Format(CultureInfo.InvariantCulture, "({0}, {1}{2})", Latitude, Longitude, alt);
    }
}
