namespace Here.Sdk.Common.Routing;

/// <summary>Classification of a place along a route.</summary>
public enum RoutePlaceType
{
    /// <summary>Unknown type.</summary>
    Unknown = 0,
    /// <summary>Airport.</summary>
    Airport = 1,
    /// <summary>Bus stop or station.</summary>
    Bus = 2,
    /// <summary>Car train terminal.</summary>
    CarTrain = 3,
    /// <summary>Ferry terminal.</summary>
    Ferry = 4,
    /// <summary>Hiking / trail point.</summary>
    Hike = 5,
    /// <summary>Park-and-ride location.</summary>
    Park = 6,
    /// <summary>Public transport hub.</summary>
    PublicTransport = 7,
    /// <summary>Route start.</summary>
    Start = 8,
    /// <summary>Intermediate stop.</summary>
    Stop = 9,
    /// <summary>Route end / destination.</summary>
    End = 10,
}
