using System;

namespace Here.Sdk.Premium.Common.Errors;

/// <summary>Raised when a network-level failure prevents the request from completing.</summary>
public sealed class HereNetworkException : HereException
{
    /// <summary>Initializes a new <see cref="HereNetworkException"/>.</summary>
    public HereNetworkException(string message)
        : base(message, HereErrorCode.NetworkFailure) { }

    /// <summary>Initializes a new <see cref="HereNetworkException"/> with an inner exception.</summary>
    public HereNetworkException(string message, Exception innerException)
        : base(message, innerException, HereErrorCode.NetworkFailure) { }
}
