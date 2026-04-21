namespace Here.Sdk.Common.Navigation;

/// <summary>Method used to collect tolls at a toll point.</summary>
public enum TollCollectionMethod
{
    /// <summary>Exact change cash only.</summary>
    ExactCash = 0,
    /// <summary>Mixed cash (change given).</summary>
    MixedCash = 1,
    /// <summary>Multi-lane facility.</summary>
    MultiLane = 2,
    /// <summary>Open-road tolling (no barrier).</summary>
    OpenRoad = 3,
    /// <summary>Bank card payment.</summary>
    BankCard = 4,
}
