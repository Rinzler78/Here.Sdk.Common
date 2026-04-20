namespace Here.Sdk.Premium.Common.Search;

/// <summary>Access restriction of an EV charging station.</summary>
public enum EvAccessType
{
    /// <summary>Open to the general public.</summary>
    Public = 0,
    /// <summary>Restricted access (e.g. employee-only).</summary>
    Restricted = 1,
    /// <summary>Private use only.</summary>
    Private = 2,
    /// <summary>Test or development station.</summary>
    Test = 3,
}
