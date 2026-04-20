namespace Here.Sdk.Premium.Common.Routing;

/// <summary>Driving maneuver action at a route instruction point.</summary>
public enum ManeuverAction
{
    /// <summary>Depart from origin.</summary>
    Depart = 0,
    /// <summary>Arrive at destination.</summary>
    Arrive = 1,
    /// <summary>U-turn to the left.</summary>
    LeftUTurn = 2,
    /// <summary>Sharp left turn.</summary>
    SharpLeftTurn = 3,
    /// <summary>Standard left turn.</summary>
    LeftTurn = 4,
    /// <summary>Slight left turn.</summary>
    SlightLeftTurn = 5,
    /// <summary>Continue straight ahead.</summary>
    Continue = 6,
    /// <summary>Slight right turn.</summary>
    SlightRightTurn = 7,
    /// <summary>Standard right turn.</summary>
    RightTurn = 8,
    /// <summary>Sharp right turn.</summary>
    SharpRightTurn = 9,
    /// <summary>U-turn to the right.</summary>
    RightUTurn = 10,
    /// <summary>Enter a roundabout.</summary>
    RoundaboutEnter = 11,
    /// <summary>Pass through a roundabout exit without leaving.</summary>
    RoundaboutPass = 12,
    /// <summary>Exit at the 1st roundabout exit.</summary>
    RoundaboutExit51 = 51,
    /// <summary>Exit at the 2nd roundabout exit.</summary>
    RoundaboutExit52 = 52,
    /// <summary>Exit at the 3rd roundabout exit.</summary>
    RoundaboutExit53 = 53,
    /// <summary>Exit at the 4th roundabout exit.</summary>
    RoundaboutExit54 = 54,
    /// <summary>Exit at the 5th roundabout exit.</summary>
    RoundaboutExit55 = 55,
    /// <summary>Exit at the 6th roundabout exit.</summary>
    RoundaboutExit56 = 56,
    /// <summary>Exit at the 7th roundabout exit.</summary>
    RoundaboutExit57 = 57,
    /// <summary>Exit at the 8th roundabout exit.</summary>
    RoundaboutExit58 = 58,
    /// <summary>Exit at the 9th roundabout exit.</summary>
    RoundaboutExit59 = 59,
    /// <summary>Exit at the 10th roundabout exit.</summary>
    RoundaboutExit510 = 510,
    /// <summary>Board a ferry.</summary>
    Ferry = 100,
    /// <summary>Enter a highway (motorway on-ramp).</summary>
    EnterHighway = 101,
    /// <summary>Leave a highway (motorway off-ramp).</summary>
    LeaveHighway = 102,
    /// <summary>Cross a border.</summary>
    BorderCrossing = 103,
}
