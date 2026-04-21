using System.Globalization;

namespace Here.Sdk.Common.Geometry;

/// <summary>Immutable 3D point in Cartesian space.</summary>
public readonly record struct Point3D
{
    /// <summary>X coordinate.</summary>
    public double X { get; }

    /// <summary>Y coordinate.</summary>
    public double Y { get; }

    /// <summary>Z coordinate.</summary>
    public double Z { get; }

    /// <summary>Initializes a new <see cref="Point3D"/>.</summary>
    public Point3D(double x, double y, double z) { X = x; Y = y; Z = z; }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", X, Y, Z);
}
