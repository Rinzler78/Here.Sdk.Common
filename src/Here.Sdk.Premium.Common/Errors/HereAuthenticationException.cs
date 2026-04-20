using System;

namespace Here.Sdk.Premium.Common.Errors;

/// <summary>Raised when authentication or authorization with the HERE platform fails.</summary>
public sealed class HereAuthenticationException : HereException
{
    /// <summary>Initializes a new <see cref="HereAuthenticationException"/>.</summary>
    public HereAuthenticationException(string message)
        : base(message, HereErrorCode.AuthenticationFailure) { }

    /// <summary>Initializes a new <see cref="HereAuthenticationException"/> with an inner exception.</summary>
    public HereAuthenticationException(string message, Exception innerException)
        : base(message, innerException, HereErrorCode.AuthenticationFailure) { }
}
