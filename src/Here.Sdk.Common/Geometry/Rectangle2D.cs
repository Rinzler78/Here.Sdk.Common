namespace Here.Sdk.Common.Geometry;

/// <summary>Axis-aligned rectangle defined by an origin point and a size.</summary>
public readonly record struct Rectangle2D
{
    /// <summary>Top-left origin.</summary>
    public Point2D Origin { get; }

    /// <summary>Dimensions.</summary>
    public Size2D Size { get; }

    /// <summary>Initializes a new <see cref="Rectangle2D"/>.</summary>
    public Rectangle2D(Point2D origin, Size2D size) { Origin = origin; Size = size; }
}
