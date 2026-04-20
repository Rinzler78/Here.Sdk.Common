namespace Here.Sdk.Common.Map;

/// <summary>Visual map rendering scheme.</summary>
public enum MapScheme
{
    /// <summary>Standard day map. Default value.</summary>
    NormalDay = 0,
    /// <summary>Standard night map.</summary>
    NormalNight = 1,
    /// <summary>Satellite imagery (day).</summary>
    SatelliteDay = 2,
    /// <summary>Hybrid (satellite + labels) day.</summary>
    HybridDay = 3,
    /// <summary>Hybrid (satellite + labels) night.</summary>
    HybridNight = 4,
    /// <summary>Lightweight day map.</summary>
    LiteDay = 5,
    /// <summary>Lightweight night map.</summary>
    LiteNight = 6,
    /// <summary>Lightweight hybrid day map.</summary>
    LiteHybridDay = 7,
}
