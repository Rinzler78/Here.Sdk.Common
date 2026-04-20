using System;

namespace Here.Sdk.Premium.Common.Geography;

/// <summary>3-axis orientation described by bearing, tilt, and roll angles.</summary>
public sealed record GeoOrientation
{
    /// <summary>Bearing in degrees, normalized to [0, 360). <c>null</c> if unknown.</summary>
    public double? BearingInDegrees { get; }

    /// <summary>Tilt (pitch) in degrees, clamped to [-90, 90]. <c>null</c> if unknown.</summary>
    public double? TiltInDegrees { get; }

    /// <summary>Roll in degrees, clamped to [-180, 180]. <c>null</c> if unknown.</summary>
    public double? RollInDegrees { get; }

    /// <summary>Initializes a new <see cref="GeoOrientation"/> with optional angle values.</summary>
    public GeoOrientation(double? bearingInDegrees = null, double? tiltInDegrees = null, double? rollInDegrees = null)
    {
        BearingInDegrees = bearingInDegrees.HasValue
            ? ((bearingInDegrees.Value % 360.0) + 360.0) % 360.0
            : null;

        TiltInDegrees = tiltInDegrees.HasValue
            ? Math.Max(-90.0, Math.Min(90.0, tiltInDegrees.Value))
            : null;

        RollInDegrees = rollInDegrees.HasValue
            ? Math.Max(-180.0, Math.Min(180.0, rollInDegrees.Value))
            : null;
    }
}
