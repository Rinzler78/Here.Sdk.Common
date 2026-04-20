namespace Here.Sdk.Premium.Common.Navigation;

/// <summary>Classification of a navigation milestone.</summary>
public enum MilestoneType
{
    /// <summary>A stop-over waypoint where the user halts.</summary>
    StopOver = 0,
    /// <summary>A pass-through waypoint.</summary>
    PassThrough = 1,
    /// <summary>An intermediate waypoint.</summary>
    Waypoint = 2,
    /// <summary>Final destination.</summary>
    Destination = 3,
}
