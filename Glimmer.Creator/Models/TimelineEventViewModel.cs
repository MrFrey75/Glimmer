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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
