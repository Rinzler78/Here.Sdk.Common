using System.Globalization;

namespace Here.Sdk.Common.Geometry;

/// <summary>Immutable 2D point in Cartesian space.</summary>
public readonly record struct Point2D
{
    /// <summary>X coordinate.</summary>
    public double X { get; }

    /// <summary>Y coordinate.</summary>
    public double Y { get; }

    /// <summary>Initializes a new <see cref="Point2D"/>.</summary>
    public Point2D(double x, double y) { X = x; Y = y; }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "({0}, {1})", X, Y);
}
