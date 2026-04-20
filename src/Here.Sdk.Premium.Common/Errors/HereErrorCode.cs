namespace Here.Sdk.Premium.Common.Errors;

/// <summary>Structured error codes for HERE SDK operations.</summary>
public enum HereErrorCode
{
    /// <summary>No error.</summary>
    None = 0,

    /// <summary>A network-level failure occurred.</summary>
    NetworkFailure = 1,

    /// <summary>Authentication or authorization failed.</summary>
    AuthenticationFailure = 2,

    /// <summary>The request parameters were invalid.</summary>
    InvalidRequest = 3,

    /// <summary>The requested resource was not found.</summary>
    NotFound = 4,

    /// <summary>The request was rate-limited by the server.</summary>
    RateLimited = 5,

    /// <summary>The account quota has been exceeded.</summary>
    QuotaExceeded = 6,

    /// <summary>The operation timed out.</summary>
    Timeout = 7,

    /// <summary>The operation was cancelled.</summary>
    Cancelled = 8,

    /// <summary>An unknown or unclassified error occurred.</summary>
    Unknown = 9,
}
