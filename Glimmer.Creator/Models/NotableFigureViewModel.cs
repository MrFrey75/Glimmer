using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;

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
    
    // Species reference
    public Guid? SpeciesId { get; set; }
    
    // Physical characteristics
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? Length { get; set; }
    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;
    public LengthMeasureEnum LengthMeasure { get; set; } = LengthMeasureEnum.Unknown;
    public string? EyeColor { get; set; }
    public string? HairColor { get; set; }
    public string? SkinColor { get; set; }
    public string? DistinguishingFeatures { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public SexualOrientationEnum SexualOrientation { get; set; } = SexualOrientationEnum.Unknown;
    
    // Magical abilities
    public bool HasMagicalAbilities { get; set; }
    public string? MagicalAbilitiesDescription { get; set; }
    
    // Biographical
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    
    // Additional information
    public string? Occupation { get; set; }
    public string? NotableAchievements { get; set; }
    public string? Biography { get; set; }
    public string? AdditionalNotes { get; set; }
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
    
    // Species reference
    public Guid? SpeciesId { get; set; }
    
    // Physical characteristics
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? Length { get; set; }
    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;
    public LengthMeasureEnum LengthMeasure { get; set; } = LengthMeasureEnum.Unknown;
    public string? EyeColor { get; set; }
    public string? HairColor { get; set; }
    public string? SkinColor { get; set; }
    public string? DistinguishingFeatures { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public SexualOrientationEnum SexualOrientation { get; set; } = SexualOrientationEnum.Unknown;
    
    // Magical abilities
    public bool HasMagicalAbilities { get; set; }
    public string? MagicalAbilitiesDescription { get; set; }
    
    // Biographical
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    
    // Additional information
    public string? Occupation { get; set; }
    public string? NotableAchievements { get; set; }
    public string? Biography { get; set; }
    public string? AdditionalNotes { get; set; }
    
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
    
    // Species reference
    public Species? Species { get; set; }
    
    // Physical characteristics
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? Length { get; set; }
    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;
    public LengthMeasureEnum LengthMeasure { get; set; } = LengthMeasureEnum.Unknown;
    public string? EyeColor { get; set; }
    public string? HairColor { get; set; }
    public string? SkinColor { get; set; }
    public string? DistinguishingFeatures { get; set; }
    public GenderEnum Gender { get; set; }
    public SexualOrientationEnum SexualOrientation { get; set; }
    
    // Magical abilities
    public bool HasMagicalAbilities { get; set; }
    public string? MagicalAbilitiesDescription { get; set; }
    
    // Biographical
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    
    // Additional information
    public string? Occupation { get; set; }
    public string? NotableAchievements { get; set; }
    public string? Biography { get; set; }
    public string? AdditionalNotes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
