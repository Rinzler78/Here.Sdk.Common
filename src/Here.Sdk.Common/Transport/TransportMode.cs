namespace Here.Sdk.Common.Transport;

/// <summary>Mode of transport for routing and navigation.</summary>
public enum TransportMode
{
    /// <summary>Passenger car.</summary>
    Car = 0,
    /// <summary>Truck or heavy goods vehicle.</summary>
    Truck = 1,
    /// <summary>Pedestrian.</summary>
    Pedestrian = 2,
    /// <summary>Bicycle.</summary>
    Bicycle = 3,
    /// <summary>Scooter or moped.</summary>
    Scooter = 4,
    /// <summary>Taxi.</summary>
    Taxi = 5,
    /// <summary>Public bus.</summary>
    Bus = 6,
    /// <summary>Private bus or coach.</summary>
    PrivateBus = 7,
    /// <summary>Ferry.</summary>
    Ferry = 8,
}
