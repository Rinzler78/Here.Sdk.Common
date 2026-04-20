namespace Here.Sdk.Common.Navigation;

/// <summary>Current status of a navigation milestone.</summary>
public enum MilestoneStatus
{
    /// <summary>Milestone has been reached (user is at or past the milestone point).</summary>
    Reached = 0,
    /// <summary>Milestone has been passed (user has moved beyond it).</summary>
    Passed = 1,
}
