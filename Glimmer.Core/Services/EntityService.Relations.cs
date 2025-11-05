using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - EntityRelation Operations
/// </summary>
public partial class EntityService
{
public async Task<EntityRelation?> CreateRelationAsync(Guid universeId, BaseEntity fromEntity, BaseEntity toEntity, RelationTypeEnum relationType)
    {
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var relation = new EntityRelation(universeId, fromEntity, toEntity, relationType)
        {
            Oid = await _relationRepository.GetNextIdAsync()
        };

        await _relationRepository.CreateAsync(relation);
        
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        
        return relation;
    }

    public async Task<EntityRelation?> GetRelationByIdAsync(Guid universeId, int relationId)
    {
        return await _relationRepository.GetByIdAsync(relationId);
    }

    public async Task<List<EntityRelation>> GetRelationsByUniverseAsync(Guid universeId)
    {
        return await _relationRepository.GetByUniverseIdAsync(universeId);
    }

    public async Task<List<EntityRelation>> GetRelationsByEntityAsync(Guid universeId, Guid entityId)
    {
        return await _relationRepository.GetByEntityIdAsync(universeId, entityId);
    }

    public async Task<bool> UpdateRelationAsync(Guid universeId, EntityRelation relation)
    {
        var existing = await GetRelationByIdAsync(universeId, relation.Oid);
        if (existing == null) return false;

        existing.RelationType = relation.RelationType;
        existing.UpdatedAt = DateTime.UtcNow;
        await _relationRepository.UpdateAsync(existing);

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

        return true;
    }

    public async Task<bool> DeleteRelationAsync(Guid universeId, int relationId)
    {
        var success = await _relationRepository.DeleteAsync(relationId);
        if (!success) return false;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

        return true;
    }
}
