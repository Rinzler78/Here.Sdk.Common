namespace Here.Sdk.Premium.Common.Geography;

/// <summary>WGS84 Earth constants used for geodetic calculations.</summary>
public static class EarthConstants
{
    /// <summary>Mean radius of the Earth in meters (WGS84).</summary>
    public const double MeanRadiusMeters = 6_371_000.0;

    /// <summary>Semi-major axis in meters (WGS84).</summary>
    public const double SemiMajorAxisMeters = 6_378_137.0;

    /// <summary>Semi-minor axis in meters (WGS84).</summary>
    public const double SemiMinorAxisMeters = 6_356_752.314245;
}
