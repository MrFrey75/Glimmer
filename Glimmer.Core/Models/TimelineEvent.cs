using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class TimelineEvent : BaseEntity
{
    public string Title => Name;
    public TimelineEventTypeEnum EventType { get; set; } = TimelineEventTypeEnum.Unknown;
}
