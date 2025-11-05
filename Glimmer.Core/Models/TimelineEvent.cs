using Glimmer.Core.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Glimmer.Core.Models;

[BsonIgnoreExtraElements]
public class TimelineEvent : BaseEntity
{
    public string Title => Name;
    public TimelineEventTypeEnum EventType { get; set; } = TimelineEventTypeEnum.Unknown;
    
    /// <summary>
    /// Determines if this event uses calendar-based or relative dating
    /// </summary>
    public bool UseCalendarDate { get; set; } = true;

    public bool IsAnchorEvent { get; set; } = false; // Marks if this event can be used as an anchor for relative dating
    
    /// <summary>
    /// Calendar-based date (only used if UseCalendarDate is true)
    /// Format: Year, Month, Day as nullable integers for flexible precision
    /// </summary>
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public int? Second { get; set; }
    
    /// <summary>
    /// Era or calendar system name (e.g., "CE", "AE", "Third Age")
    /// </summary>
    public string Era { get; set; } = string.Empty;
    
    /// <summary>
    /// Relative dating: Reference to anchor event UUID (only used if UseCalendarDate is false)
    /// </summary>
    public Guid? AnchorEventId { get; set; }
    
    /// <summary>
    /// Relative dating: Years before (-) or after (+) the anchor event
    /// </summary>
    public int? YearsFromAnchor { get; set; }
    public int? MonthsFromAnchor { get; set; }
    public int? WeeksFromAnchor { get; set; }
    public int? DaysFromAnchor { get; set; }
    public int? HoursFromAnchor { get; set; }
    public int? MinutesFromAnchor { get; set; }
    public int? SecondsFromAnchor { get; set; }

    
    /// <summary>
    /// Human-readable description of relative timing (e.g., "10 years before the Great War")
    /// </summary>
    public string RelativeDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Duration of the event in days (optional)
    /// </summary>
    public int? DurationInYears { get; set; }
    public int? DurationInMonths { get; set; }
    public int? DurationInWeeks { get; set; }
    public int? DurationInDays { get; set; }
    public int? DurationInHours { get; set; }
    public int? DurationInMinutes { get; set; }
    public int? DurationInSeconds { get; set; }
}
