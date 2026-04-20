namespace Here.Sdk.Premium.Common.Search;

/// <summary>Type of place returned by a search result.</summary>
public enum PlaceType
{
    /// <summary>Unknown place type.</summary>
    Unknown = 0,
    /// <summary>Administrative area.</summary>
    Area = 1,
    /// <summary>Locality (city, town, village).</summary>
    Locality = 2,
    /// <summary>Street.</summary>
    Street = 3,
    /// <summary>House number.</summary>
    HouseNumber = 4,
    /// <summary>Point of interest.</summary>
    PointOfInterest = 5,
}
