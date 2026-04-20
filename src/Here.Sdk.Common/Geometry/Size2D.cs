using System;
using System.Globalization;

namespace Here.Sdk.Common.Geometry;

/// <summary>Immutable 2D size with non-negative width and height.</summary>
public readonly record struct Size2D
{
    /// <summary>Width. Must be &gt;= 0.</summary>
    public double Width { get; }

    /// <summary>Height. Must be &gt;= 0.</summary>
    public double Height { get; }

    /// <summary>Initializes a new <see cref="Size2D"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">When width or height is negative.</exception>
    public Size2D(double width, double height)
    {
        if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Width must be >= 0.");
        if (height < 0) throw new ArgumentOutOfRangeException(nameof(height), "Height must be >= 0.");
        Width = width;
        Height = height;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "{0}x{1}", Width, Height);
}
