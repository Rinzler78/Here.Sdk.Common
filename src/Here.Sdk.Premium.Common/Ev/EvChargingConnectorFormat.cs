namespace Here.Sdk.Premium.Common.Ev;

/// <summary>Physical format of an EV charging connector.</summary>
public enum EvChargingConnectorFormat
{
    /// <summary>Socket (no cable attached).</summary>
    Socket = 0,
    /// <summary>Tethered cable attached to the charger.</summary>
    Cable = 1,
}
