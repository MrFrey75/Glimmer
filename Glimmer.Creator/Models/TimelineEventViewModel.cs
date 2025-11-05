using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;

namespace Glimmer.Creator.Models;

public class TimelineEventListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<TimelineEventCardViewModel> Events { get; set; } = new();
}

public class TimelineEventCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimelineEventTypeEnum EventType { get; set; }
    public bool UseCalendarDate { get; set; } = true;
    public bool IsAnchorEvent { get; set; }
    
    // For display purposes
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public string Era { get; set; } = string.Empty;
    public string RelativeDescription { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
}

public class CreateTimelineEventViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Event name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Event type is required")]
    public TimelineEventTypeEnum EventType { get; set; } = TimelineEventTypeEnum.Discovery;

    public bool UseCalendarDate { get; set; } = true;
    public bool IsAnchorEvent { get; set; } = false;
    
    // Calendar-based dating
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public int? Second { get; set; }
    
    [StringLength(50, ErrorMessage = "Era cannot exceed 50 characters")]
    public string Era { get; set; } = string.Empty;
    
    // Relative dating
    public Guid? AnchorEventId { get; set; }
    public int? YearsFromAnchor { get; set; }
    public int? MonthsFromAnchor { get; set; }
    public int? WeeksFromAnchor { get; set; }
    public int? DaysFromAnchor { get; set; }
    public int? HoursFromAnchor { get; set; }
    public int? MinutesFromAnchor { get; set; }
    public int? SecondsFromAnchor { get; set; }
    
    [StringLength(200, ErrorMessage = "Relative description cannot exceed 200 characters")]
    public string RelativeDescription { get; set; } = string.Empty;
    
    // Duration
    public int? DurationInYears { get; set; }
    public int? DurationInMonths { get; set; }
    public int? DurationInWeeks { get; set; }
    public int? DurationInDays { get; set; }
    public int? DurationInHours { get; set; }
    public int? DurationInMinutes { get; set; }
    public int? DurationInSeconds { get; set; }
}

public class EditTimelineEventViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Event name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Event type is required")]
    public TimelineEventTypeEnum EventType { get; set; }
    
    public bool UseCalendarDate { get; set; } = true;
    public bool IsAnchorEvent { get; set; }
    
    // Calendar-based dating
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public int? Second { get; set; }
    
    [StringLength(50, ErrorMessage = "Era cannot exceed 50 characters")]
    public string Era { get; set; } = string.Empty;
    
    // Relative dating
    public Guid? AnchorEventId { get; set; }
    public int? YearsFromAnchor { get; set; }
    public int? MonthsFromAnchor { get; set; }
    public int? WeeksFromAnchor { get; set; }
    public int? DaysFromAnchor { get; set; }
    public int? HoursFromAnchor { get; set; }
    public int? MinutesFromAnchor { get; set; }
    public int? SecondsFromAnchor { get; set; }
    
    [StringLength(200, ErrorMessage = "Relative description cannot exceed 200 characters")]
    public string RelativeDescription { get; set; } = string.Empty;
    
    // Duration
    public int? DurationInYears { get; set; }
    public int? DurationInMonths { get; set; }
    public int? DurationInWeeks { get; set; }
    public int? DurationInDays { get; set; }
    public int? DurationInHours { get; set; }
    public int? DurationInMinutes { get; set; }
    public int? DurationInSeconds { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TimelineEventDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimelineEventTypeEnum EventType { get; set; }
    
    public bool UseCalendarDate { get; set; } = true;
    public bool IsAnchorEvent { get; set; }
    
    // Calendar-based dating
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Day { get; set; }
    public int? Hour { get; set; }
    public int? Minute { get; set; }
    public int? Second { get; set; }
    public string Era { get; set; } = string.Empty;
    
    // Relative dating
    public Guid? AnchorEventId { get; set; }
    public string? AnchorEventName { get; set; }
    public int? YearsFromAnchor { get; set; }
    public int? MonthsFromAnchor { get; set; }
    public int? WeeksFromAnchor { get; set; }
    public int? DaysFromAnchor { get; set; }
    public int? HoursFromAnchor { get; set; }
    public int? MinutesFromAnchor { get; set; }
    public int? SecondsFromAnchor { get; set; }
    public string RelativeDescription { get; set; } = string.Empty;
    
    // Duration
    public int? DurationInYears { get; set; }
    public int? DurationInMonths { get; set; }
    public int? DurationInWeeks { get; set; }
    public int? DurationInDays { get; set; }
    public int? DurationInHours { get; set; }
    public int? DurationInMinutes { get; set; }
    public int? DurationInSeconds { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // List of available anchor events for dropdown
    public List<TimelineEventCardViewModel> AvailableAnchorEvents { get; set; } = new();
}
