using System;

namespace Here.Sdk.Premium.Common.Localization;

/// <summary>ISO 3166-1 alpha-3 country code (e.g. <c>"DEU"</c>, <c>"USA"</c>).</summary>
public readonly record struct CountryCode
{
    /// <summary>Country code value.</summary>
    public string Value { get; }

    /// <summary>Initializes a new <see cref="CountryCode"/>.</summary>
    /// <exception cref="ArgumentException">When <paramref name="value"/> is null or empty.</exception>
    public CountryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Country code must not be empty.", nameof(value));
        Value = value;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
