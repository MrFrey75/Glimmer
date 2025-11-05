using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Artifact : BaseEntity
{
    public ArtifactTypeEnum ArtifactType { get; set; } = ArtifactTypeEnum.Unknown;

    // Physical Characteristics
    public MaterialCompositionEnmum MaterialComposition { get; set; } = MaterialCompositionEnmum.Unknown;
    public string? Dimensions { get; set; } = 
    public string? Weight { get; set; }
    public string? Color { get; set; }
    public ConditionEnum Condition { get; set; } = ConditionEnum.Unknown;

    // Historical and Cultural Significance
    public string? Origin { get; set; }
    public string? HistoricalPeriod { get; set; }
    public string? CulturalSignificance { get; set; }
    public string? NotableOwners { get; set; }

    // Magical or Special Properties
    public bool HasMagicalProperties { get; set; } = false;
    public string? MagicalPropertiesDescription { get; set; }

    // Additional Information
    public string? AdditionalNotes { get; set; }
}

public enum MaterialCompositionEnmum
{
    Unknown = 0,
    Wood = 1,
    Metal = 2,
    Stone = 3,
    Fabric = 4,
    Glass = 5,
    Composite = 6,
    Organic = 7,
    Synthetic = 8
}

public enum ConditionEnum
{
    Unknown = 0,
    Pristine = 1,
    Good = 2,
    Fair = 3,
    Poor = 4,
    Damaged = 5,
    Ruined = 6
}
