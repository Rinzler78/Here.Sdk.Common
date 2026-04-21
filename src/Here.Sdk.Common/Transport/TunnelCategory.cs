namespace Here.Sdk.Common.Transport;

/// <summary>ADR tunnel category restricting transport of certain dangerous goods.</summary>
public enum TunnelCategory
{
    /// <summary>Category A — no restriction.</summary>
    A = 0,
    /// <summary>Category B — explosives and gas-related restrictions.</summary>
    B = 1,
    /// <summary>Category C — stricter restrictions on explosives.</summary>
    C = 2,
    /// <summary>Category D — restrictions on all dangerous goods except limited quantities.</summary>
    D = 3,
    /// <summary>Category E — most restrictive category.</summary>
    E = 4,
}
