using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;

namespace Glimmer.Creator.Models;

public class ArtifactListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<ArtifactCardViewModel> Artifacts { get; set; } = new();
}

public class ArtifactCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ArtifactTypeEnum ArtifactType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateArtifactViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Artifact name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Artifact type is required")]
    public ArtifactTypeEnum ArtifactType { get; set; } = ArtifactTypeEnum.Relic;

    // Physical Characteristics
    public MaterialCompositionEnmum MaterialComposition { get; set; } = MaterialCompositionEnmum.Unknown;
    
    [StringLength(100)]
    public string Dimensions { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Weight { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Color { get; set; } = string.Empty;
    
    public ConditionEnum Condition { get; set; } = ConditionEnum.Unknown;

    // Historical and Cultural Significance
    [StringLength(200)]
    public string Origin { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string HistoricalPeriod { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string CulturalSignificance { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string NotableOwners { get; set; } = string.Empty;

    // Magical or Special Properties
    public bool HasMagicalProperties { get; set; } = false;
    
    [StringLength(1000)]
    public string MagicalPropertiesDescription { get; set; } = string.Empty;

    // Additional Information
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
}

public class EditArtifactViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Artifact name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Artifact type is required")]
    public ArtifactTypeEnum ArtifactType { get; set; }
    
    // Physical Characteristics
    public MaterialCompositionEnmum MaterialComposition { get; set; } = MaterialCompositionEnmum.Unknown;
    
    [StringLength(100)]
    public string Dimensions { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Weight { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Color { get; set; } = string.Empty;
    
    public ConditionEnum Condition { get; set; } = ConditionEnum.Unknown;

    // Historical and Cultural Significance
    [StringLength(200)]
    public string Origin { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string HistoricalPeriod { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string CulturalSignificance { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string NotableOwners { get; set; } = string.Empty;

    // Magical or Special Properties
    public bool HasMagicalProperties { get; set; } = false;
    
    [StringLength(1000)]
    public string MagicalPropertiesDescription { get; set; } = string.Empty;

    // Additional Information
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ArtifactDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ArtifactTypeEnum ArtifactType { get; set; }
    
    // Physical Characteristics
    public MaterialCompositionEnmum MaterialComposition { get; set; }
    public string Dimensions { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public ConditionEnum Condition { get; set; }

    // Historical and Cultural Significance
    public string Origin { get; set; } = string.Empty;
    public string HistoricalPeriod { get; set; } = string.Empty;
    public string CulturalSignificance { get; set; } = string.Empty;
    public string NotableOwners { get; set; } = string.Empty;

    // Magical or Special Properties
    public bool HasMagicalProperties { get; set; }
    public string MagicalPropertiesDescription { get; set; } = string.Empty;

    // Additional Information
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
