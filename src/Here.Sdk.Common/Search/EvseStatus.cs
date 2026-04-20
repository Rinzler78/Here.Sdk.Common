namespace Here.Sdk.Common.Search;

/// <summary>Operational status of an EV supply equipment (EVSE) unit.</summary>
public enum EvseStatus
{
    /// <summary>Connector is available for charging.</summary>
    Available = 0,
    /// <summary>Connector is occupied by a vehicle.</summary>
    Occupied = 1,
    /// <summary>Connector is out of service.</summary>
    OutOfService = 2,
    /// <summary>Status is unknown.</summary>
    Unknown = 3,
}
