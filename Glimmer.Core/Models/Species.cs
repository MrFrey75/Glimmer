using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Species : BaseEntity
{
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Unknown;
}
