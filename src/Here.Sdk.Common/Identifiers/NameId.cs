using System;

namespace Here.Sdk.Common.Identifiers;

/// <summary>Identifier combining a human-readable name and a machine-readable id.</summary>
public sealed record NameId
{
    /// <summary>Human-readable name.</summary>
    public string Name { get; }

    /// <summary>Machine-readable identifier.</summary>
    public string Id { get; }

    /// <summary>Initializes a new <see cref="NameId"/>.</summary>
    /// <exception cref="ArgumentException">When <paramref name="name"/> or <paramref name="id"/> is empty.</exception>
    public NameId(string name, string id)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name must not be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("Id must not be empty.", nameof(id));
        Name = name;
        Id = id;
    }
}
