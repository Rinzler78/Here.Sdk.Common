using System;
using System.Globalization;

namespace Here.Sdk.Premium.Common.Units;

/// <summary>Immutable distance value with conversion helpers.</summary>
public readonly record struct Distance
{
    private const double MetersPerKilometer = 1_000.0;
    private const double MetersPerMile = 1_609.344;

    /// <summary>A zero-length distance.</summary>
    public static readonly Distance Zero = new(0.0);

    /// <summary>Value in meters.</summary>
    public double Meters { get; }

    /// <summary>Initializes a new <see cref="Distance"/> with the given meter value.</summary>
    public Distance(double meters) => Meters = meters;

    /// <summary>Creates a <see cref="Distance"/> from kilometers.</summary>
    public static Distance FromKilometers(double km) => new(km * MetersPerKilometer);

    /// <summary>Converts this distance to kilometers.</summary>
    public double ToKilometers() => Meters / MetersPerKilometer;

    /// <summary>Creates a <see cref="Distance"/> from statute miles.</summary>
    public static Distance FromMiles(double miles) => new(miles * MetersPerMile);

    /// <summary>Converts this distance to statute miles.</summary>
    public double ToMiles() => Meters / MetersPerMile;

    /// <summary>Adds two distances.</summary>
    public static Distance operator +(Distance a, Distance b) => new(a.Meters + b.Meters);

    /// <summary>Subtracts two distances.</summary>
    public static Distance operator -(Distance a, Distance b) => new(a.Meters - b.Meters);

    /// <summary>Scales a distance by a scalar.</summary>
    public static Distance operator *(Distance d, double scalar) => new(d.Meters * scalar);

    /// <summary>Scales a distance by a scalar.</summary>
    public static Distance operator *(double scalar, Distance d) => new(d.Meters * scalar);

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "{0} m", Meters);
}
