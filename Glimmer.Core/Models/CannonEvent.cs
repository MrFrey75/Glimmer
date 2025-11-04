using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class CannonEvent : BaseEntity
{
    public string Title => Name;
    public CannonEventTypeEnum EventType { get; set; } = CannonEventTypeEnum.Unknown;
}
