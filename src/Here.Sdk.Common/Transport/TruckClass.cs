namespace Here.Sdk.Common.Transport;

/// <summary>US Federal Highway Administration truck class (1–8).</summary>
public enum TruckClass
{
    /// <summary>Class 1 (GVW up to 6,000 lb).</summary>
    Class1 = 1,
    /// <summary>Class 2 (GVW 6,001–10,000 lb).</summary>
    Class2 = 2,
    /// <summary>Class 3 (GVW 10,001–14,000 lb).</summary>
    Class3 = 3,
    /// <summary>Class 4 (GVW 14,001–16,000 lb).</summary>
    Class4 = 4,
    /// <summary>Class 5 (GVW 16,001–19,500 lb).</summary>
    Class5 = 5,
    /// <summary>Class 6 (GVW 19,501–26,000 lb).</summary>
    Class6 = 6,
    /// <summary>Class 7 (GVW 26,001–33,000 lb).</summary>
    Class7 = 7,
    /// <summary>Class 8 (GVW over 33,000 lb).</summary>
    Class8 = 8,
}
