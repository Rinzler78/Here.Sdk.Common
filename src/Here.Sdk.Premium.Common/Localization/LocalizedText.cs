using System;

namespace Here.Sdk.Premium.Common.Localization;

/// <summary>A text value paired with its language.</summary>
public sealed record LocalizedText
{
    /// <summary>Text content.</summary>
    public string Text { get; }

    /// <summary>Language of the text.</summary>
    public LanguageCode Language { get; }

    /// <summary>Initializes a new <see cref="LocalizedText"/>.</summary>
    public LocalizedText(string text, LanguageCode language)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Language = language;
    }
}
