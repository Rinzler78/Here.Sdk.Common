using System;

namespace Here.Sdk.Premium.Common.Transport;

/// <summary>Hazardous material classes that a vehicle may carry (combinable flags).</summary>
[Flags]
public enum HazardousMaterial
{
    /// <summary>No hazardous materials.</summary>
    None = 0,
    /// <summary>Explosives (ADR Class 1).</summary>
    Explosive = 1 << 0,
    /// <summary>Gases (ADR Class 2).</summary>
    Gas = 1 << 1,
    /// <summary>Flammable liquids (ADR Class 3).</summary>
    Flammable = 1 << 2,
    /// <summary>Flammable solids (ADR Class 4).</summary>
    Combustible = 1 << 3,
    /// <summary>Organic peroxides (ADR Class 5.2).</summary>
    Organic = 1 << 4,
    /// <summary>Toxic substances (ADR Class 6.1).</summary>
    Poison = 1 << 5,
    /// <summary>Radioactive materials (ADR Class 7).</summary>
    Radioactive = 1 << 6,
    /// <summary>Corrosives (ADR Class 8).</summary>
    Corrosive = 1 << 7,
    /// <summary>Poisonous inhalation hazard.</summary>
    PoisonousInhalation = 1 << 8,
    /// <summary>Substances harmful to the aquatic environment.</summary>
    HarmfulToWater = 1 << 9,
    /// <summary>Other hazardous materials not listed above.</summary>
    Other = 1 << 10,
}
