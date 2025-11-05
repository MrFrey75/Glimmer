using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Fact : BaseEntity
{
    public string Value { get; set; } = string.Empty;

    public FactTypeEnum FactType { get; set; } = FactTypeEnum.Unknown;

    // Additional Information
    public string? AdditionalNotes { get; set; }
}
