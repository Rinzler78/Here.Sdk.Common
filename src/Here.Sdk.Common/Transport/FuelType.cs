namespace Here.Sdk.Common.Transport;

/// <summary>Vehicle fuel or propulsion type.</summary>
public enum FuelType
{
    /// <summary>Diesel fuel.</summary>
    Diesel = 0,
    /// <summary>Petrol (gasoline).</summary>
    Petrol = 1,
    /// <summary>Liquefied petroleum gas.</summary>
    LPG = 2,
    /// <summary>Compressed natural gas.</summary>
    CNG = 3,
    /// <summary>Liquefied natural gas.</summary>
    LNG = 4,
    /// <summary>Hydrogen fuel cell.</summary>
    Hydrogen = 5,
    /// <summary>Battery electric.</summary>
    Electric = 6,
    /// <summary>Diesel-electric hybrid.</summary>
    HybridDiesel = 7,
    /// <summary>Petrol-electric hybrid.</summary>
    HybridPetrol = 8,
}
