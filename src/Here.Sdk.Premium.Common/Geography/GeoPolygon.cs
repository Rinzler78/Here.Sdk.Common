using System;
using System.Collections.Generic;

namespace Here.Sdk.Premium.Common.Geography;

/// <summary>Closed polygon defined by at least 3 vertices.</summary>
public sealed record GeoPolygon
{
    /// <summary>Vertex list. Must have at least 3 entries.</summary>
    public IReadOnlyList<GeoCoordinates> Vertices { get; }

    /// <summary>Initializes a new <see cref="GeoPolygon"/>.</summary>
    /// <exception cref="ArgumentException">When fewer than 3 vertices are provided.</exception>
    public GeoPolygon(IReadOnlyList<GeoCoordinates> vertices)
    {
        if (vertices is null) throw new ArgumentNullException(nameof(vertices));
        if (vertices.Count < 3)
            throw new ArgumentException("A polygon requires at least 3 vertices.", nameof(vertices));
        Vertices = vertices;
    }
}
