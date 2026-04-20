namespace Here.Sdk.Premium.Common.Navigation;

/// <summary>Speed warning state relative to the current speed limit.</summary>
public enum SpeedWarningStatus
{
    /// <summary>Vehicle is exceeding the speed limit.</summary>
    SpeedLimitExceeded = 0,
    /// <summary>Vehicle has returned to within the speed limit.</summary>
    SpeedLimitRestored = 1,
}
