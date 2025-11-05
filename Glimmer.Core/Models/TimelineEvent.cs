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
    
    /// <summary>
    /// Calendar-based date (only used if UseCalendarDate is true)
    /// Format: Year, Month, Day as nullable integers for flexible precision
    /// </summary>
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    
    /// <summary>
    /// Era or calendar system name (e.g., "CE", "AE", "Third Age")
    /// </summary>
    public string? Era { get; set; }
    
    /// <summary>
    /// Relative dating: Reference to anchor event UUID (only used if UseCalendarDate is false)
    /// </summary>
    public Guid? AnchorEventId { get; set; }
    
    /// <summary>
    /// Relative dating: Years before (-) or after (+) the anchor event
    /// </summary>
    public int? YearsFromAnchor { get; set; }
    
    /// <summary>
    /// Human-readable description of relative timing (e.g., "10 years before the Great War")
    /// </summary>
    public string? RelativeDescription { get; set; }
    
    /// <summary>
    /// Duration of the event in days (optional)
    /// </summary>
    public int? DurationInDays { get; set; }
}
