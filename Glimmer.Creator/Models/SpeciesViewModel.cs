using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;

namespace Glimmer.Creator.Models;

public class SpeciesListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<SpeciesCardViewModel> Species { get; set; } = new();
}

public class SpeciesCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SpeciesTypeEnum SpeciesType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSpeciesViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Species name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Species type is required")]
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Mammal;
    
    // Physical Characteristics
    [StringLength(100)]
    public string AverageHeight { get; set; } = string.Empty;
    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    [StringLength(100)]
    public string AverageWeight { get; set; } = string.Empty;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;
    [StringLength(100)]
    public string AverageLength { get; set; } = string.Empty;
    public LengthMeasureEnum LengthMeasure { get; set; } = LengthMeasureEnum.Unknown;
    [StringLength(100)]
    public string SkinColor { get; set; } = string.Empty;
    [StringLength(100)]
    public string EyeColor { get; set; } = string.Empty;
    [StringLength(100)]
    public string HairColor { get; set; } = string.Empty;
    [StringLength(500)]
    public string DistinguishingFeatures { get; set; } = string.Empty;
    
    // Habitat and Distribution
    public NaturalHabitatEnum NaturalHabitat { get; set; } = NaturalHabitatEnum.Unknown;
    public GeographicDistributionEnum GeographicDistribution { get; set; } = GeographicDistributionEnum.Unknown;
    
    // Behavior and Social Structure
    public SocialStructureEnum SocialStructure { get; set; } = SocialStructureEnum.Unknown;
    [StringLength(1000)]
    public string BehavioralTraits { get; set; } = string.Empty;
    
    // Diet and Lifespan
    public DietTypeEnum Diet { get; set; } = DietTypeEnum.Unknown;
    [StringLength(100)]
    public string AverageLifespan { get; set; } = string.Empty;
    
    // Reproduction
    public ReproductiveMethodEnum ReproductiveMethods { get; set; } = ReproductiveMethodEnum.Unknown;
    [StringLength(100)]
    public string GestationPeriod { get; set; } = string.Empty;
    [StringLength(100)]
    public string OffspringPerBirth { get; set; } = string.Empty;
    
    // Additional Information
    public CommunicationMethodEnum CommunicationMethods { get; set; } = CommunicationMethodEnum.Unknown;
    [StringLength(1000)]
    public string PredatorsAndThreats { get; set; } = string.Empty;
    [StringLength(500)]
    public string ConservationStatus { get; set; } = string.Empty;
    
    // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    [StringLength(1000)]
    public string MagicalAbilitiesDescription { get; set; } = string.Empty;
    
    // Notes
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
}

public class EditSpeciesViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Species name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Species type is required")]
    public SpeciesTypeEnum SpeciesType { get; set; }
    
    // Physical Characteristics
    [StringLength(100)]
    public string AverageHeight { get; set; } = string.Empty;
    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;

    [StringLength(100)]
    public string AverageWeight { get; set; } = string.Empty;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;


    [StringLength(100)]
    public string AverageLength { get; set; } = string.Empty;
    public LengthMeasureEnum LengthMeasure { get; set; } = LengthMeasureEnum.Unknown;


    [StringLength(100)]
    public string SkinColor { get; set; } = string.Empty;
    [StringLength(100)]
    public string EyeColor { get; set; } = string.Empty;
    [StringLength(100)]
    public string HairColor { get; set; } = string.Empty;
    [StringLength(500)]
    public string DistinguishingFeatures { get; set; } = string.Empty;
    
    // Habitat and Distribution
    public NaturalHabitatEnum NaturalHabitat { get; set; } = NaturalHabitatEnum.Unknown;
    public GeographicDistributionEnum GeographicDistribution { get; set; } = GeographicDistributionEnum.Unknown;
    
    // Behavior and Social Structure
    public SocialStructureEnum SocialStructure { get; set; } = SocialStructureEnum.Unknown;
    [StringLength(1000)]
    public string BehavioralTraits { get; set; } = string.Empty;
    
    // Diet and Lifespan
    public DietTypeEnum Diet { get; set; } = DietTypeEnum.Unknown;
    [StringLength(100)]
    public string AverageLifespan { get; set; } = string.Empty;
    
    // Reproduction
    public ReproductiveMethodEnum ReproductiveMethods { get; set; } = ReproductiveMethodEnum.Unknown;
    [StringLength(100)]
    public string GestationPeriod { get; set; } = string.Empty;
    [StringLength(100)]
    public string OffspringPerBirth { get; set; } = string.Empty;
    
    // Additional Information
    public CommunicationMethodEnum CommunicationMethods { get; set; } = CommunicationMethodEnum.Unknown;
    [StringLength(1000)]
    public string PredatorsAndThreats { get; set; } = string.Empty;
    [StringLength(500)]
    public string ConservationStatus { get; set; } = string.Empty;
    
    // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    [StringLength(1000)]
    public string MagicalAbilitiesDescription { get; set; } = string.Empty;
    
    // Notes
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SpeciesDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SpeciesTypeEnum SpeciesType { get; set; }
    
    // Physical Characteristics
    public string AverageHeight { get; set; } = string.Empty;
    public HeightMeasureEnum HeightMeasure { get; set; }
    public string AverageWeight { get; set; } = string.Empty;
    public WeightMeasureEnum WeightMeasure { get; set; }
    public string AverageLength { get; set; } = string.Empty;
    public LengthMeasureEnum LengthMeasure { get; set; }
    public string SkinColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string DistinguishingFeatures { get; set; } = string.Empty;
    
    // Habitat and Distribution
    public NaturalHabitatEnum NaturalHabitat { get; set; }
    public GeographicDistributionEnum GeographicDistribution { get; set; }
    
    // Behavior and Social Structure
    public SocialStructureEnum SocialStructure { get; set; }
    public string BehavioralTraits { get; set; } = string.Empty;
    
    // Diet and Lifespan
    public DietTypeEnum Diet { get; set; }
    public string AverageLifespan { get; set; } = string.Empty;
    
    // Reproduction
    public ReproductiveMethodEnum ReproductiveMethods { get; set; }
    public string GestationPeriod { get; set; } = string.Empty;
    public string OffspringPerBirth { get; set; } = string.Empty;
    
    // Additional Information
    public CommunicationMethodEnum CommunicationMethods { get; set; }
    public string PredatorsAndThreats { get; set; } = string.Empty;
    public string ConservationStatus { get; set; } = string.Empty;
    
    // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; }
    public string MagicalAbilitiesDescription { get; set; } = string.Empty;
    
    // Notes
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
