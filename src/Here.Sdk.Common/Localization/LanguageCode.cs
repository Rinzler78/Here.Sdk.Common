using System;

namespace Here.Sdk.Common.Localization;

/// <summary>ISO 639-1 or ISO 639-3 language code.</summary>
public readonly record struct LanguageCode
{
    /// <summary>Language code value (e.g. <c>"en"</c>, <c>"fra"</c>).</summary>
    public string Value { get; }

    /// <summary>Initializes a new <see cref="LanguageCode"/>.</summary>
    /// <exception cref="ArgumentException">When <paramref name="value"/> is null or empty.</exception>
    public LanguageCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Language code must not be empty.", nameof(value));
        Value = value;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
