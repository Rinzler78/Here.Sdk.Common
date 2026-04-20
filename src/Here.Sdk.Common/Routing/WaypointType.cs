namespace Here.Sdk.Common.Routing;

/// <summary>How a waypoint is treated during route calculation.</summary>
public enum WaypointType
{
    /// <summary>Route must stop at this waypoint.</summary>
    StopOver = 0,
    /// <summary>Route may pass through this waypoint without stopping.</summary>
    PassThrough = 1,
}
