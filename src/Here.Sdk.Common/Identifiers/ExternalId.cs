using System;

namespace Here.Sdk.Common.Identifiers;

/// <summary>External identifier scoped to a named authority (provider).</summary>
public sealed record ExternalId
{
    /// <summary>Authority that issued the identifier (e.g. <c>"here_place_id"</c>, <c>"wikidata"</c>).</summary>
    public string Provider { get; }

    /// <summary>Identifier value within the provider's namespace.</summary>
    public string Id { get; }

    /// <summary>Initializes a new <see cref="ExternalId"/>.</summary>
    /// <exception cref="ArgumentException">When <paramref name="provider"/> or <paramref name="id"/> is empty.</exception>
    public ExternalId(string provider, string id)
    {
        if (string.IsNullOrWhiteSpace(provider)) throw new ArgumentException("Provider must not be empty.", nameof(provider));
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id must not be empty.", nameof(id));
        Provider = provider;
        Id = id;
    }
}
