namespace Here.Sdk.Common.Traffic;

/// <summary>Type of traffic incident.</summary>
public enum TrafficIncidentType
{
    /// <summary>Unknown incident type.</summary>
    Unknown = 0,
    /// <summary>Road accident.</summary>
    Accident = 1,
    /// <summary>Traffic congestion.</summary>
    Congestion = 2,
    /// <summary>Disabled or broken-down vehicle.</summary>
    DisabledVehicle = 3,
    /// <summary>Mass transit disruption.</summary>
    MassTransit = 4,
    /// <summary>Miscellaneous incident.</summary>
    Miscellaneous = 5,
    /// <summary>Other news item affecting traffic.</summary>
    OtherNews = 6,
    /// <summary>Planned event (concert, sporting event, etc.).</summary>
    PlannedEvent = 7,
    /// <summary>Road hazard.</summary>
    RoadHazard = 8,
    /// <summary>Road construction or works.</summary>
    Construction = 9,
    /// <summary>Lane restriction.</summary>
    LaneRestriction = 10,
    /// <summary>Weather-related impact.</summary>
    Weather = 11,
}
