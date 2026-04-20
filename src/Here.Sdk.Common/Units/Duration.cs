using System;
using System.Globalization;

namespace Here.Sdk.Common.Units;

/// <summary>Immutable duration value wrapping <see cref="TimeSpan"/>.</summary>
public readonly record struct Duration
{
    /// <summary>A zero duration.</summary>
    public static readonly Duration Zero = new(TimeSpan.Zero);

    /// <summary>Underlying <see cref="TimeSpan"/> value.</summary>
    public TimeSpan Value { get; }

    /// <summary>Initializes a new <see cref="Duration"/>.</summary>
    public Duration(TimeSpan value) => Value = value;

    /// <summary>Creates a <see cref="Duration"/> from seconds.</summary>
    public static Duration FromSeconds(double seconds) => new(TimeSpan.FromSeconds(seconds));

    /// <summary>Creates a <see cref="Duration"/> from minutes.</summary>
    public static Duration FromMinutes(double minutes) => new(TimeSpan.FromMinutes(minutes));

    /// <summary>Total seconds.</summary>
    public double TotalSeconds => Value.TotalSeconds;

    /// <summary>Adds two durations.</summary>
    public static Duration operator +(Duration a, Duration b) => new(a.Value + b.Value);

    /// <summary>Subtracts two durations.</summary>
    public static Duration operator -(Duration a, Duration b) => new(a.Value - b.Value);

    /// <inheritdoc/>
    public override string ToString() =>
        Value.ToString("c", CultureInfo.InvariantCulture);
}
