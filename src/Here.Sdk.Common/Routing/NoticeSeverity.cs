namespace Here.Sdk.Common.Routing;

/// <summary>Severity of a route notice or warning.</summary>
public enum NoticeSeverity
{
    /// <summary>Critical issue that prevents travel.</summary>
    Critical = 0,
    /// <summary>Non-critical warning.</summary>
    Warning = 1,
    /// <summary>Informational notice.</summary>
    Info = 2,
}
