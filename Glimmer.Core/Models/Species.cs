using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Species : BaseEntity
{
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Unknown;

    // Physical Characteristics
    public string? AverageHeight { get; set; }
    public string? AverageWeight { get; set; }
    public string? SkinColor { get; set; }
    public string? EyeColor { get; set; }
    public string? HairColor { get; set; }
    public string? DistinguishingFeatures { get; set; } 

    // Habitat and Distribution
    public NaturalHabitatEnum NaturalHabitat { get; set; } = NaturalHabitatEnum.Unknown;
    public GeographicDistributionEnum GeographicDistribution { get; set; } = GeographicDistributionEnum.Unknown;

    // Behavior and Social Structure
    public SocialStructureEnunm SocialStructure { get; set; } = SocialStructureEnunm.Unknown;
    public string? BehavioralTraits { get; set; }   

    // Diet and Lifespan
    public DietTypeEnum Diet { get; set; } = DietTypeEnum.Unknown; 
    public string? AverageLifespan { get; set; }

    // Reproduction
    public ReproductiveMethodEnum ReproductiveMethods { get; set; } = ReproductiveMethodEnum.Unknown;
    public string? GestationPeriod { get; set; }
    public string? OffspringPerBirth { get; set; }

    // Additional Information
    public CommunicationMethodEnum CommunicationMethods { get; set; } = CommunicationMethodEnum.Unknown;
    public string? PredatorsAndThreats { get; set; }
    public string? ConservationStatus { get; set; }

    // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    public string? MagicalAbilitiesDescription { get; set; }

    // Notes
    public string? AdditionalNotes { get; set; }
}

public enum SocialStructureEnunm
{
    Unknown = 0,
    Solitary = 1,
    PairBonded = 2,
    Pack = 3,
    Herd = 4,
    Colony = 5,
    CasteBased = 6
}

public enum ReproductiveMethodEnum
{
    Unknown = 0,
    Oviparous = 1,
    Viviparous = 2,
    Ovoviviparous = 3,
    Asexual = 4
}

public enum CommunicationMethodEnum
{
    Unknown = 0,
    Vocalizations = 1,
    BodyLanguage = 2,
    ChemicalSignals = 3,
    VisualSignals = 4,
    ElectromagneticSignals = 5
}

public enum GeographicDistributionEnum
{
    Unknown = 0,
    Global = 1,
    Continental = 2,
    Regional = 3,
    Endemic = 4
}

public enum NaturalHabitatEnum
{
    Unknown = 0,
    Forest = 1,
    Grassland = 2,
    Desert = 3,
    Aquatic = 4,
    Mountain = 5,
    Urban = 6,
    Wetland = 7,
    Tundra = 8,
    Jungle = 9
}

public enum DietTypeEnum
{
    Unknown = 0,
    Herbivore = 1,
    Carnivore = 2,
    Omnivore = 3,
    Insectivore = 4,
    Piscivore = 5,
    Detritivore = 6,
    Frugivore = 7,
    Granivore = 8
}
