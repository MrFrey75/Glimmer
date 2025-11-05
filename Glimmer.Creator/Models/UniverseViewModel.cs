using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;

namespace Glimmer.Creator.Models;

public class UniverseListViewModel
{
    public List<UniverseCardViewModel> Universes { get; set; } = new();
}

public class UniverseCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int EntityCount { get; set; }
}

public class CreateUniverseViewModel
{
    [Required(ErrorMessage = "Universe name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Timeline type is required")]
    public TimelineTypeEnum TimelineType { get; set; } = TimelineTypeEnum.CalendarBased;
}

public class EditUniverseViewModel
{
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Universe name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Timeline type is required")]
    public TimelineTypeEnum TimelineType { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UniverseDetailsViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimelineTypeEnum TimelineType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedByUsername { get; set; } = string.Empty;
    
    public int FigureCount { get; set; }
    public int LocationCount { get; set; }
    public int ArtifactCount { get; set; }
    public int EventCount { get; set; }
    public int FactionCount { get; set; }
    public int FactCount { get; set; }
    public int SpeciesCount { get; set; }
    public int TotalEntityCount { get; set; }
}
