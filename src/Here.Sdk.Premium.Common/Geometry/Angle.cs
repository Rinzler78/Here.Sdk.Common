using System;
using System.Globalization;

namespace Here.Sdk.Premium.Common.Geometry;

/// <summary>Angle value with degree and radian conversions.</summary>
public readonly record struct Angle
{
    private const double RadToDeg = 180.0 / Math.PI;
    private const double DegToRad = Math.PI / 180.0;

    /// <summary>Angle value in degrees.</summary>
    public double ValueInDegrees { get; }

    /// <summary>Initializes a new <see cref="Angle"/> from degrees.</summary>
    public Angle(double valueInDegrees) => ValueInDegrees = valueInDegrees;

    /// <summary>Creates an <see cref="Angle"/> from radians.</summary>
    public static Angle FromRadians(double radians) => new(radians * RadToDeg);

    /// <summary>Converts this angle to radians.</summary>
    public double ToRadians() => ValueInDegrees * DegToRad;

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "{0}\u00b0", ValueInDegrees);
}
