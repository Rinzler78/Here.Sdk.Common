namespace Here.Sdk.Common.Ev;

/// <summary>
/// Detailed EVSE (Electric Vehicle Supply Equipment) operational state.
/// <c>Unknown</c> is NOT ordinal 0 to ensure uninitialized values surface as errors.
/// </summary>
public enum EvseState
{
    /// <summary>EVSE is available for use.</summary>
    Available = 0,
    /// <summary>EVSE is blocked (physically obstructed or reserved).</summary>
    Blocked = 1,
    /// <summary>EVSE is actively charging a vehicle.</summary>
    Charging = 2,
    /// <summary>EVSE is inoperative (fault or maintenance).</summary>
    Inoperative = 3,
    /// <summary>EVSE is out of order (hardware fault).</summary>
    OutOfOrder = 4,
    /// <summary>EVSE is planned but not yet operational.</summary>
    Planned = 5,
    /// <summary>EVSE has been removed.</summary>
    Removed = 6,
    /// <summary>EVSE is reserved for a specific user.</summary>
    Reserved = 7,
    /// <summary>EVSE state is unknown.</summary>
    Unknown = 8,
}
