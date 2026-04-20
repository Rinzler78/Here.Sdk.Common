namespace Here.Sdk.Common.Positioning;

/// <summary>
/// Power/accuracy trade-off for positioning requests.
/// <c>NavigationAccuracy</c> is ordinal 0 and the safe default when switching on this enum.
/// </summary>
public enum LocationAccuracy
{
    /// <summary>Highest accuracy mode — uses GPS and all available sensors. Safe default.</summary>
    NavigationAccuracy = 0,
    /// <summary>Best available accuracy for the current power budget.</summary>
    BestAvailable = 1,
    /// <summary>Balanced accuracy and power consumption.</summary>
    BalancedPowerAccuracy = 2,
    /// <summary>Low-power mode — reduced accuracy.</summary>
    LowPowerAccuracy = 3,
}
