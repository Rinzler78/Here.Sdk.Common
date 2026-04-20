using System;

namespace Here.Sdk.Common.Geometry;

/// <summary>Closed integer range [Min, Max].</summary>
public readonly record struct IntegerRange
{
    /// <summary>Inclusive lower bound.</summary>
    public int Min { get; }

    /// <summary>Inclusive upper bound.</summary>
    public int Max { get; }

    /// <summary>Initializes a new <see cref="IntegerRange"/>.</summary>
    /// <exception cref="ArgumentException">When <paramref name="min"/> &gt; <paramref name="max"/>.</exception>
    public IntegerRange(int min, int max)
    {
        if (min > max)
            throw new ArgumentException($"Min ({min}) must be <= Max ({max}).", nameof(min));
        Min = min;
        Max = max;
    }
}
