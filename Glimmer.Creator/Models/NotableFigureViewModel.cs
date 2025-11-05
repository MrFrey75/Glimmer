using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;

namespace Glimmer.Creator.Models;

public class NotableFigureListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<NotableFigureCardViewModel> Figures { get; set; } = new();
}

public class NotableFigureCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FigureTypeEnum FigureType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateNotableFigureViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Figure name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Figure type is required")]
    public FigureTypeEnum FigureType { get; set; } = FigureTypeEnum.Unknown;
}

public class EditNotableFigureViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Figure name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Figure type is required")]
    public FigureTypeEnum FigureType { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class NotableFigureDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FigureTypeEnum FigureType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
