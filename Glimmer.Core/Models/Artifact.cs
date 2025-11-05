using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Artifact : BaseEntity
{
    public ArtifactTypeEnum ArtifactType { get; set; } = ArtifactTypeEnum.Unknown;

    // Physical Characteristics
    public MaterialCompositionEnmum MaterialComposition { get; set; } = MaterialCompositionEnmum.Unknown;
    public string Dimensions { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public ConditionEnum Condition { get; set; } = ConditionEnum.Unknown;

    // Historical and Cultural Significance
    public string Origin { get; set; } = string.Empty;
    public string HistoricalPeriod { get; set; } = string.Empty;
    public string CulturalSignificance { get; set; } = string.Empty;
    public string NotableOwners { get; set; } = string.Empty;

    // Magical or Special Properties
    public bool HasMagicalProperties { get; set; } = false;
    public string MagicalPropertiesDescription { get; set; } = string.Empty;

    // Additional Information
    public string AdditionalNotes { get; set; } = string.Empty;
}




