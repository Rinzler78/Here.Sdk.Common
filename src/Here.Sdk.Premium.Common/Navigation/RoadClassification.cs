namespace Here.Sdk.Premium.Common.Navigation;

/// <summary>Functional classification of a road segment.</summary>
public enum RoadClassification
{
    /// <summary>Unknown classification.</summary>
    Unknown = 0,
    /// <summary>Motorway or interstate highway.</summary>
    Motorway = 1,
    /// <summary>Trunk road.</summary>
    TrunkRoad = 2,
    /// <summary>Primary road.</summary>
    PrimaryRoad = 3,
    /// <summary>Secondary road.</summary>
    SecondaryRoad = 4,
    /// <summary>Local road.</summary>
    LocalRoad = 5,
    /// <summary>Private road.</summary>
    PrivateRoad = 6,
    /// <summary>Residential street.</summary>
    Residential = 7,
}
