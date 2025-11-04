using Glimmer.Core.Models;
using MongoDB.Driver;

namespace Glimmer.Core.Repositories;

public interface IUniverseRepository
{
    Task<Universe?> GetByIdAsync(Guid universeId);
    Task<List<Universe>> GetByUserIdAsync(Guid userId);
    Task<List<Universe>> GetAllAsync();
    Task<Universe> CreateAsync(Universe universe);
    Task<bool> UpdateAsync(Universe universe);
    Task<bool> DeleteAsync(Guid universeId);
}

public class UniverseRepository : IUniverseRepository
{
    private readonly IMongoCollection<Universe> _universes;

    public UniverseRepository(IMongoDatabase database)
    {
        _universes = database.GetCollection<Universe>("universes");
        
        // Create indexes
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var indexKeys = Builders<Universe>.IndexKeys
            .Ascending(u => u.Uuid);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<Universe>(indexKeys, indexOptions);

        var userIndexKeys = Builders<Universe>.IndexKeys
            .Ascending("CreatedBy.Uuid");
        var userIndexModel = new CreateIndexModel<Universe>(userIndexKeys);

        try
        {
            _universes.Indexes.CreateOne(indexModel);
            _universes.Indexes.CreateOne(userIndexModel);
        }
        catch
        {
            // Indexes might already exist
        }
    }

    public async Task<Universe?> GetByIdAsync(Guid universeId)
    {
        return await _universes.Find(u => u.Uuid == universeId).FirstOrDefaultAsync();
    }

    public async Task<List<Universe>> GetByUserIdAsync(Guid userId)
    {
        return await _universes.Find(u => u.CreatedBy.Uuid == userId).ToListAsync();
    }

    public async Task<List<Universe>> GetAllAsync()
    {
        return await _universes.Find(_ => true).ToListAsync();
    }

    public async Task<Universe> CreateAsync(Universe universe)
    {
        await _universes.InsertOneAsync(universe);
        return universe;
    }

    public async Task<bool> UpdateAsync(Universe universe)
    {
        var result = await _universes.ReplaceOneAsync(u => u.Uuid == universe.Uuid, universe);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid universeId)
    {
        var result = await _universes.DeleteOneAsync(u => u.Uuid == universeId);
        return result.DeletedCount > 0;
    }
}
