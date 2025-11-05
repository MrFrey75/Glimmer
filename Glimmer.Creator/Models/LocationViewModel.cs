using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;

namespace Glimmer.Creator.Models;

public class LocationListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<LocationCardViewModel> Locations { get; set; } = new();
}

public class LocationCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LocationTypeEnum LocationType { get; set; }
    public string? ParentLocationName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateLocationViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Location name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Location type is required")]
    public LocationTypeEnum LocationType { get; set; } = LocationTypeEnum.CityTown;
    
    public Guid? ParentLocationId { get; set; }
    public List<LocationSelectItem> AvailableParentLocations { get; set; } = new();
}

public class EditLocationViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Location name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Location type is required")]
    public LocationTypeEnum LocationType { get; set; }
    
    public Guid? ParentLocationId { get; set; }
    public List<LocationSelectItem> AvailableParentLocations { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class LocationDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LocationTypeEnum LocationType { get; set; }
    public string? ParentLocationName { get; set; }
    public Guid? ParentLocationId { get; set; }
    public List<LocationCardViewModel> ChildLocations { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class LocationSelectItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LocationTypeEnum Type { get; set; }
}
