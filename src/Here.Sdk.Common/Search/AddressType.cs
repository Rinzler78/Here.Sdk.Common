namespace Here.Sdk.Common.Search;

/// <summary>Granularity level of an address result.</summary>
public enum AddressType
{
    /// <summary>Unknown granularity.</summary>
    Unknown = 0,
    /// <summary>Exact point match.</summary>
    Point = 1,
    /// <summary>House number match.</summary>
    HouseNumber = 2,
    /// <summary>Street-level match.</summary>
    Street = 3,
    /// <summary>Postal code match.</summary>
    PostalCode = 4,
    /// <summary>Locality level 1 (city/municipality).</summary>
    LocalityLevel1 = 5,
    /// <summary>Locality level 2 (district).</summary>
    LocalityLevel2 = 6,
    /// <summary>Locality level 3 (neighborhood).</summary>
    LocalityLevel3 = 7,
    /// <summary>Country-level match.</summary>
    Country = 8,
}
