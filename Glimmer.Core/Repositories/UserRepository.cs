using Glimmer.Core.Models;
using MongoDB.Driver;

namespace Glimmer.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid userId);
    Task<List<User>> GetAllAsync();
}

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(IMongoDatabase database)
    {
        _users = database.GetCollection<User>("users");
        
        // Create indexes
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        var indexKeys = Builders<User>.IndexKeys
            .Ascending(u => u.Username);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<User>(indexKeys, indexOptions);
        
        var emailIndexKeys = Builders<User>.IndexKeys
            .Ascending(u => u.Email);
        var emailIndexOptions = new CreateIndexOptions { Unique = true };
        var emailIndexModel = new CreateIndexModel<User>(emailIndexKeys, emailIndexOptions);

        try
        {
            _users.Indexes.CreateOne(indexModel);
            _users.Indexes.CreateOne(emailIndexModel);
        }
        catch
        {
            // Indexes might already exist
        }
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _users.Find(u => u.Uuid == userId).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var result = await _users.ReplaceOneAsync(u => u.Uuid == user.Uuid, user);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var result = await _users.DeleteOneAsync(u => u.Uuid == userId);
        return result.DeletedCount > 0;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _users.Find(_ => true).ToListAsync();
    }
}
