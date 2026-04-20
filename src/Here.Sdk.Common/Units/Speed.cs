using System.Globalization;

namespace Here.Sdk.Common.Units;

/// <summary>Immutable speed value with conversion helpers.</summary>
public readonly record struct Speed
{
    private const double KphFactor = 3.6;
    private const double MphFactor = 2.23693629;

    /// <summary>A zero speed.</summary>
    public static readonly Speed Zero = new(0.0);

    /// <summary>Value in meters per second.</summary>
    public double MetersPerSecond { get; }

    /// <summary>Initializes a new <see cref="Speed"/>.</summary>
    public Speed(double metersPerSecond) => MetersPerSecond = metersPerSecond;

    /// <summary>Creates a <see cref="Speed"/> from km/h.</summary>
    public static Speed FromKph(double kph) => new(kph / KphFactor);

    /// <summary>Converts this speed to km/h.</summary>
    public double ToKph() => MetersPerSecond * KphFactor;

    /// <summary>Creates a <see cref="Speed"/> from mph.</summary>
    public static Speed FromMph(double mph) => new(mph / MphFactor);

    /// <summary>Converts this speed to mph.</summary>
    public double ToMph() => MetersPerSecond * MphFactor;

    /// <inheritdoc/>
    public override string ToString() =>
        string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} m/s", MetersPerSecond);
}
