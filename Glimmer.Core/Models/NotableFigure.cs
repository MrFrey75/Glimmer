using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class NotableFigure : BaseEntity
{
    public FigureTypeEnum FigureType { get; set; } = FigureTypeEnum.Unknown;
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Mammal;
}
