using System;
using Here.Sdk.Premium.Common.Geography;

namespace Here.Sdk.Premium.Common.Positioning;

/// <summary>Timestamped geographic position with optional sensor fields.</summary>
public sealed record Location
{
    /// <summary>Geographic coordinates.</summary>
    public GeoCoordinates Coordinates { get; }

    /// <summary>UTC timestamp of the fix.</summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>Bearing in degrees [0, 360), or <c>null</c> if unknown.</summary>
    public double? BearingInDegrees { get; init; }

    /// <summary>Speed in m/s, or <c>null</c> if unknown.</summary>
    public double? SpeedInMetersPerSecond { get; init; }

    /// <summary>Horizontal accuracy radius in meters, or <c>null</c> if unknown.</summary>
    public double? HorizontalAccuracyInMeters { get; init; }

    /// <summary>Vertical accuracy in meters, or <c>null</c> if unknown.</summary>
    public double? VerticalAccuracyInMeters { get; init; }

    /// <summary>Bearing accuracy in degrees, or <c>null</c> if unknown.</summary>
    public double? BearingAccuracyInDegrees { get; init; }

    /// <summary>Speed accuracy in m/s, or <c>null</c> if unknown.</summary>
    public double? SpeedAccuracyInMetersPerSecond { get; init; }

    /// <summary>Initializes a new <see cref="Location"/> with required fields.</summary>
    public Location(GeoCoordinates coordinates, DateTimeOffset timestamp)
    {
        Coordinates = coordinates;
        Timestamp = timestamp;
    }
}
