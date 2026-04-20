using System.Globalization;

namespace Here.Sdk.Common.Geography;

/// <summary>Bearing angle normalized to [0, 360) degrees from north.</summary>
// PERF: hot path — allocation budget: 0 bytes, target ≤5 ns
public readonly record struct GeoBearing
{
    /// <summary>Bearing in degrees from north, normalized to [0, 360).</summary>
    public double DegreesFromNorth { get; }

    /// <summary>Initializes a new <see cref="GeoBearing"/>, normalizing <paramref name="degreesFromNorth"/> to [0, 360).</summary>
    public GeoBearing(double degreesFromNorth)
    {
        DegreesFromNorth = ((degreesFromNorth % 360.0) + 360.0) % 360.0;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        DegreesFromNorth.ToString("F2", CultureInfo.InvariantCulture) + "\u00b0";
}
