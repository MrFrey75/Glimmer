using Glimmer.Core.Enums;
using Glimmer.Core.Models;

namespace Glimmer.Creator.Models;

public class EntityRelationViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public int? RelationId { get; set; }
    public Guid? FromEntityId { get; set; }
    public string FromEntityName { get; set; } = string.Empty;
    public string FromEntityType { get; set; } = string.Empty;
    public Guid? ToEntityId { get; set; }
    public string ToEntityName { get; set; } = string.Empty;
    public string ToEntityType { get; set; } = string.Empty;
    public RelationTypeEnum? RelationType { get; set; }
    public string? Description { get; set; }
}

public class EntityRelationListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<EntityRelationCardViewModel> Relations { get; set; } = new();
}

public class EntityRelationCardViewModel
{
    public int RelationId { get; set; }
    public string FromEntityName { get; set; } = string.Empty;
    public string FromEntityType { get; set; } = string.Empty;
    public string ToEntityName { get; set; } = string.Empty;
    public string ToEntityType { get; set; } = string.Empty;
    public RelationTypeEnum RelationType { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EntityPickerItem
{
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
}
