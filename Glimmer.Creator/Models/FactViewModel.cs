using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;

namespace Glimmer.Creator.Models;

public class FactListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<FactCardViewModel> Facts { get; set; } = new();
}

public class FactCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FactTypeEnum FactType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFactViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Fact name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
    public string Value { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Fact type is required")]
    public FactTypeEnum FactType { get; set; } = FactTypeEnum.Historical;
    
    [StringLength(2000, ErrorMessage = "Additional notes cannot exceed 2000 characters")]
    public string AdditionalNotes { get; set; } = string.Empty;
}

public class EditFactViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Fact name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Value cannot exceed 500 characters")]
    public string Value { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Fact type is required")]
    public FactTypeEnum FactType { get; set; }
    
    [StringLength(2000, ErrorMessage = "Additional notes cannot exceed 2000 characters")]
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class FactDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public FactTypeEnum FactType { get; set; }
    public string AdditionalNotes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
