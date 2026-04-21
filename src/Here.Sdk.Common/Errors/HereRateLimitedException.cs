using System;

namespace Here.Sdk.Common.Errors;

/// <summary>Raised when the HERE platform rate-limits the caller.</summary>
public sealed class HereRateLimitedException : HereException
{
    /// <summary>Suggested retry-after duration, or <c>null</c> if not provided by the server.</summary>
    public TimeSpan? RetryAfter { get; }

    /// <summary>Initializes a new <see cref="HereRateLimitedException"/>.</summary>
    public HereRateLimitedException(string message, TimeSpan? retryAfter = null)
        : base(message, HereErrorCode.RateLimited) => RetryAfter = retryAfter;
}
