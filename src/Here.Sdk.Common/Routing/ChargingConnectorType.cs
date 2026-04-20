namespace Here.Sdk.Common.Routing;

/// <summary>EV charging connector standard.</summary>
public enum ChargingConnectorType
{
    /// <summary>Unknown connector type.</summary>
    Unknown = 0,
    /// <summary>IEC 62196-2 Type 1 CCS (Combined Charging System, SAE J1772 combo).</summary>
    IEC62196T1Combo = 1,
    /// <summary>IEC 62196-2 Type 2 (Mennekes).</summary>
    IEC62196T2 = 2,
    /// <summary>IEC 62196-2 Type 2 CCS.</summary>
    IEC62196T2Combo = 3,
    /// <summary>IEC 62196-2 Type 3A (French).</summary>
    IEC62196T3A = 4,
    /// <summary>IEC 62196-2 Type 3C (Scame).</summary>
    IEC62196T3C = 5,
    /// <summary>Tesla proprietary connector.</summary>
    Tesla = 6,
    /// <summary>GB/T 20234 Part 2 (Chinese AC).</summary>
    GBT20234Part2 = 7,
    /// <summary>GB/T 20234 Part 3 (Chinese DC).</summary>
    GBT20234Part3 = 8,
    /// <summary>CHAdeMO DC fast charge.</summary>
    CHAdeMO = 9,
    /// <summary>Domestic/household connector.</summary>
    Domestic = 10,
    /// <summary>Type 1 CCS combo.</summary>
    Type1Combo = 11,
    /// <summary>SAE J3400 (NACS — North American Charging Standard).</summary>
    SaeJ3400 = 12,
}
