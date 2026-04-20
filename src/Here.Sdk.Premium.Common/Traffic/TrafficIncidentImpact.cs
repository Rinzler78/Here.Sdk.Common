namespace Here.Sdk.Premium.Common.Traffic;

/// <summary>
/// Severity of a traffic incident's impact on travel time.
/// Ordinals are ordered from least severe to most severe to support comparisons.
/// </summary>
public enum TrafficIncidentImpact
{
    /// <summary>Impact is unknown.</summary>
    Unknown = 0,
    /// <summary>Low impact — negligible delay.</summary>
    LowImpact = 1,
    /// <summary>Minor impact — small delay.</summary>
    Minor = 2,
    /// <summary>Major impact — significant delay.</summary>
    Major = 3,
    /// <summary>Critical impact — road impassable or very large delay.</summary>
    Critical = 4,
}
