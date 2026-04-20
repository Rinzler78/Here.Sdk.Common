namespace Here.Sdk.Common.Navigation;

/// <summary>Lane recommendation state for turn guidance.</summary>
public enum LaneRecommendationState
{
    /// <summary>Lane is not recommended for the upcoming maneuver.</summary>
    NotRecommended = 0,
    /// <summary>Lane is recommended.</summary>
    Recommended = 1,
    /// <summary>Lane is highly recommended (best option).</summary>
    HighlyRecommended = 2,
}
