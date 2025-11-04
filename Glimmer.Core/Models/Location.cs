using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Location : BaseEntity
{
    public Location? ParentLocation { get; set; } = null;
    public LocationTypeEnum LocationType { get; set; } = LocationTypeEnum.Unknown;
}
