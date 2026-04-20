using System;
using System.Globalization;

namespace Here.Sdk.Premium.Common.Geometry;

/// <summary>2D anchor point with offsets normalized to [0, 1].</summary>
public readonly record struct Anchor2D
{
    /// <summary>Horizontal offset normalized to [0, 1].</summary>
    public double HorizontalOffset { get; }

    /// <summary>Vertical offset normalized to [0, 1].</summary>
    public double VerticalOffset { get; }

    /// <summary>Initializes a new <see cref="Anchor2D"/>.</summary>
    /// <exception cref="ArgumentOutOfRangeException">When either offset is outside [0, 1].</exception>
    public Anchor2D(double horizontalOffset, double verticalOffset)
    {
        if (horizontalOffset < 0 || horizontalOffset > 1)
            throw new ArgumentOutOfRangeException(nameof(horizontalOffset), "Must be in [0, 1].");
        if (verticalOffset < 0 || verticalOffset > 1)
            throw new ArgumentOutOfRangeException(nameof(verticalOffset), "Must be in [0, 1].");
        HorizontalOffset = horizontalOffset;
        VerticalOffset = verticalOffset;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(CultureInfo.InvariantCulture, "({0}, {1})", HorizontalOffset, VerticalOffset);
}
