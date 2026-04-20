using System;

namespace Here.Sdk.Premium.Common.Errors;

/// <summary>Base exception for all HERE SDK errors.</summary>
public class HereException : Exception
{
    /// <summary>Structured error code identifying the failure category.</summary>
    public HereErrorCode Code { get; }

    /// <summary>Initializes a new <see cref="HereException"/>.</summary>
    public HereException(string message, HereErrorCode code = HereErrorCode.Unknown)
        : base(message) => Code = code;

    /// <summary>Initializes a new <see cref="HereException"/> with an inner exception.</summary>
    public HereException(string message, Exception innerException, HereErrorCode code = HereErrorCode.Unknown)
        : base(message, innerException) => Code = code;
}
