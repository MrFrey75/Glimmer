using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Faction : BaseEntity
{
    public FactionTypeEnum FactionType { get; set; } = FactionTypeEnum.Unknown;
}