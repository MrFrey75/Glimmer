using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Artifact : BaseEntity
{
    public ArtifactTypeEnum ArtifactType { get; set; } = ArtifactTypeEnum.Unknown;

    // Physical Characteristics
    public string? MaterialComposition { get; set; }
    public string? Dimensions { get; set; }
    public string? Weight { get; set; }
    public string? Color { get; set; }
    public string? Condition { get; set; }

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
