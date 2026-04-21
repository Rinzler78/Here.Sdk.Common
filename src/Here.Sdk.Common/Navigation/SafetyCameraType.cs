namespace Here.Sdk.Common.Navigation;

/// <summary>Type of safety camera along the route.</summary>
public enum SafetyCameraType
{
    /// <summary>Unknown camera type.</summary>
    Unknown = 0,
    /// <summary>Fixed speed camera.</summary>
    SpeedCamera = 1,
    /// <summary>Red-light camera.</summary>
    RedLightCamera = 2,
    /// <summary>Average speed (section) camera.</summary>
    SectionCamera = 3,
    /// <summary>Mobile speed enforcement camera.</summary>
    MobileCamera = 4,
}
