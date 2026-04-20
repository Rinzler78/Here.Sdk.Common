using System;
using System.Collections.Generic;

namespace Here.Sdk.Common.Localization;

/// <summary>Ordered collection of localized text entries with language-based lookup.</summary>
public sealed record LocalizedTexts
{
    /// <summary>Ordered list of localized text items.</summary>
    public IReadOnlyList<LocalizedText> Items { get; }

    /// <summary>Initializes a new <see cref="LocalizedTexts"/>.</summary>
    public LocalizedTexts(IReadOnlyList<LocalizedText> items)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    /// <summary>
    /// Returns the text for <paramref name="preferred"/> language, falling back to the first entry.
    /// Returns <c>null</c> when the collection is empty.
    /// </summary>
    public LocalizedText? GetText(LanguageCode preferred)
    {
        if (Items.Count == 0) return null;

        foreach (var item in Items)
        {
            if (item.Language.Value.Equals(preferred.Value, StringComparison.OrdinalIgnoreCase))
                return item;
        }

        return Items[0];
    }
}
