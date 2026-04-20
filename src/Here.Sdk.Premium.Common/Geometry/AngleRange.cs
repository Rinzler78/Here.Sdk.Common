namespace Here.Sdk.Premium.Common.Geometry;

/// <summary>Angular range defined by a start angle and an extent.</summary>
public readonly record struct AngleRange
{
    /// <summary>Start angle.</summary>
    public Angle StartAngle { get; }

    /// <summary>Angular extent (sweep).</summary>
    public Angle Extent { get; }

    /// <summary>Initializes a new <see cref="AngleRange"/>.</summary>
    public AngleRange(Angle startAngle, Angle extent) { StartAngle = startAngle; Extent = extent; }
}
