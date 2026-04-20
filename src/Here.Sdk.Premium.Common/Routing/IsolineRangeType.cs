namespace Here.Sdk.Premium.Common.Routing;

/// <summary>Dimension used to compute an isoline reachability range.</summary>
public enum IsolineRangeType
{
    /// <summary>Distance-based isoline (meters).</summary>
    Distance = 0,
    /// <summary>Time-based isoline (seconds).</summary>
    Time = 1,
    /// <summary>Energy-based isoline (kWh).</summary>
    ConsumptionInKilowattHours = 2,
}
