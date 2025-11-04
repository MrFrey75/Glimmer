using Glimmer.Core.Models;
using MongoDB.Driver;

namespace Glimmer.Core.Repositories;

public interface IRelationRepository
{
    Task<EntityRelation> CreateAsync(EntityRelation relation);
    Task<EntityRelation?> GetByIdAsync(int relationId);
    Task<List<EntityRelation>> GetByUniverseIdAsync(Guid universeId);
    Task<List<EntityRelation>> GetByEntityIdAsync(Guid universeId, Guid entityId);
    Task<bool> UpdateAsync(EntityRelation relation);
    Task<bool> DeleteAsync(int relationId);
    Task<int> GetNextIdAsync();
}

public class RelationRepository : IRelationRepository
{
    private readonly IMongoCollection<EntityRelation> _relations;

    public RelationRepository(IMongoDatabase database)
    {
        _relations = database.GetCollection<EntityRelation>("relations");
        
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var oidIndex = Builders<EntityRelation>.IndexKeys.Ascending(r => r.Oid);
        var oidIndexModel = new CreateIndexModel<EntityRelation>(oidIndex, new CreateIndexOptions { Unique = true });

        var fromEntityIndex = Builders<EntityRelation>.IndexKeys.Ascending("FromEntity.Uuid");
        var fromEntityIndexModel = new CreateIndexModel<EntityRelation>(fromEntityIndex);

        var toEntityIndex = Builders<EntityRelation>.IndexKeys.Ascending("ToEntity.Uuid");
        var toEntityIndexModel = new CreateIndexModel<EntityRelation>(toEntityIndex);

        try
        {
            _relations.Indexes.CreateOne(oidIndexModel);
            _relations.Indexes.CreateOne(fromEntityIndexModel);
            _relations.Indexes.CreateOne(toEntityIndexModel);
        }
        catch
        {
            // Indexes might already exist
        }
    }

    public async Task<EntityRelation> CreateAsync(EntityRelation relation)
    {
        await _relations.InsertOneAsync(relation);
        return relation;
    }

    public async Task<EntityRelation?> GetByIdAsync(int relationId)
    {
        return await _relations.Find(r => r.Oid == relationId && !r.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<List<EntityRelation>> GetByUniverseIdAsync(Guid universeId)
    {
        return await _relations.Find(r => r.UniverseId == universeId && !r.IsDeleted).ToListAsync();
    }

    public async Task<List<EntityRelation>> GetByEntityIdAsync(Guid universeId, Guid entityId)
    {
        var filter = Builders<EntityRelation>.Filter.And(
            Builders<EntityRelation>.Filter.Eq(r => r.UniverseId, universeId),
            Builders<EntityRelation>.Filter.Eq(r => r.IsDeleted, false),
            Builders<EntityRelation>.Filter.Or(
                Builders<EntityRelation>.Filter.Eq("FromEntity.Uuid", entityId),
                Builders<EntityRelation>.Filter.Eq("ToEntity.Uuid", entityId)
            )
        );
        
        return await _relations.Find(filter).ToListAsync();
    }

    public async Task<bool> UpdateAsync(EntityRelation relation)
    {
        var result = await _relations.ReplaceOneAsync(r => r.Oid == relation.Oid, relation);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(int relationId)
    {
        var update = Builders<EntityRelation>.Update
            .Set(r => r.IsDeleted, true)
            .Set(r => r.UpdatedAt, DateTime.UtcNow);
        
        var result = await _relations.UpdateOneAsync(r => r.Oid == relationId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<int> GetNextIdAsync()
    {
        var maxRelation = await _relations
            .Find(_ => true)
            .SortByDescending(r => r.Oid)
            .Limit(1)
            .FirstOrDefaultAsync();
        
        return maxRelation?.Oid + 1 ?? 1;
    }
}
