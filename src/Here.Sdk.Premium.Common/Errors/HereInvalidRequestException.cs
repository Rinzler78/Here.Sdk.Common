namespace Here.Sdk.Premium.Common.Errors;

/// <summary>Raised when a request contains invalid parameters.</summary>
public sealed class HereInvalidRequestException : HereException
{
    /// <summary>Name of the invalid field or parameter, or <c>null</c> if not applicable.</summary>
    public string? FieldName { get; }

    /// <summary>Initializes a new <see cref="HereInvalidRequestException"/>.</summary>
    public HereInvalidRequestException(string message, string? fieldName = null)
        : base(message, HereErrorCode.InvalidRequest) => FieldName = fieldName;
}
