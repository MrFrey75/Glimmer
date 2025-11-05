namespace Glimmer.Core.Enums;

public enum DistanceMeasureEnum
{
    Unknown = 0,

    // Metric units
    Meters = 1,
    Kilometers = 2,

    // Imperial units
    Miles = 3,
    Yards = 4,
    Feet = 5,
    Inches = 6,
    
    // Nautical units
    NauticalMiles = 7,
    Fathoms = 8,

    // stellar units could be added later
    LightYears = 9,
    Parsecs = 10

}