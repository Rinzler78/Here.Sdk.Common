namespace Here.Sdk.Premium.Common.Map;

/// <summary>Line cap style for polyline rendering.</summary>
public enum LineCap
{
    /// <summary>Flat cap — ends exactly at endpoint.</summary>
    Butt = 0,
    /// <summary>Rounded cap — semicircle at endpoint.</summary>
    Round = 1,
    /// <summary>Square cap — extends half line width past endpoint.</summary>
    Square = 2,
}
