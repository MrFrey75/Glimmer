using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Species : BaseEntity
{
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Unknown;

    // Physical Characteristics
    public string AverageHeight { get; set; } = string.Empty;
    public string AverageWeight { get; set; } = string.Empty;

    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;
    
    public string SkinColor { get; set; } = string.Empty;
    public string EyeColor { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string DistinguishingFeatures { get; set; }  = string.Empty;

    // Habitat and Distribution
    public NaturalHabitatEnum NaturalHabitat { get; set; } = NaturalHabitatEnum.Unknown;
    public GeographicDistributionEnum GeographicDistribution { get; set; } = GeographicDistributionEnum.Unknown;

    // Behavior and Social Structure
    public SocialStructureEnum SocialStructure { get; set; } = SocialStructureEnum.Unknown;
    public string BehavioralTraits { get; set; }   = string.Empty; 

    // Diet and Lifespan
    public DietTypeEnum Diet { get; set; } = DietTypeEnum.Unknown; 
    public string AverageLifespan { get; set; } = string.Empty;

    // Reproduction
    public ReproductiveMethodEnum ReproductiveMethods { get; set; } = ReproductiveMethodEnum.Unknown;
    public string GestationPeriod { get; set; } = string.Empty;
    public string OffspringPerBirth { get; set; } = string.Empty;

    // Additional Information
    public CommunicationMethodEnum CommunicationMethods { get; set; } = CommunicationMethodEnum.Unknown;
    public string PredatorsAndThreats { get; set; } = string.Empty;
    public string ConservationStatus { get; set; } = string.Empty;

    // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    public string MagicalAbilitiesDescription { get; set; } = string.Empty;

    // Notes
    public string AdditionalNotes { get; set; } = string.Empty;
}
