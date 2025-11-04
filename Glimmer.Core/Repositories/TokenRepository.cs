using Glimmer.Core.Models;
using MongoDB.Driver;

namespace Glimmer.Core.Repositories;

public interface ITokenRepository
{
    // RefreshToken operations
    Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task<bool> UpdateRefreshTokenAsync(RefreshToken token);
    Task<bool> DeleteRefreshTokensByUserIdAsync(Guid userId);
    
    // PasswordResetToken operations
    Task<PasswordResetToken> CreatePasswordResetTokenAsync(PasswordResetToken token);
    Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token);
    Task<bool> UpdatePasswordResetTokenAsync(PasswordResetToken token);
}

public class TokenRepository : ITokenRepository
{
    private readonly IMongoCollection<RefreshToken> _refreshTokens;
    private readonly IMongoCollection<PasswordResetToken> _passwordResetTokens;

    public TokenRepository(IMongoDatabase database)
    {
        _refreshTokens = database.GetCollection<RefreshToken>("refreshTokens");
        _passwordResetTokens = database.GetCollection<PasswordResetToken>("passwordResetTokens");
        
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        // RefreshToken indexes
        var tokenIndex = Builders<RefreshToken>.IndexKeys.Ascending(t => t.Token);
        var tokenIndexModel = new CreateIndexModel<RefreshToken>(tokenIndex, new CreateIndexOptions { Unique = true });
        
        var userIdIndex = Builders<RefreshToken>.IndexKeys.Ascending(t => t.UserId);
        var userIdIndexModel = new CreateIndexModel<RefreshToken>(userIdIndex);

        // PasswordResetToken indexes
        var resetTokenIndex = Builders<PasswordResetToken>.IndexKeys.Ascending(t => t.Token);
        var resetTokenIndexModel = new CreateIndexModel<PasswordResetToken>(resetTokenIndex, new CreateIndexOptions { Unique = true });

        try
        {
            _refreshTokens.Indexes.CreateOne(tokenIndexModel);
            _refreshTokens.Indexes.CreateOne(userIdIndexModel);
            _passwordResetTokens.Indexes.CreateOne(resetTokenIndexModel);
        }
        catch
        {
            // Indexes might already exist
        }
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken token)
    {
        await _refreshTokens.InsertOneAsync(token);
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _refreshTokens.Find(t => t.Token == token).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateRefreshTokenAsync(RefreshToken token)
    {
        var result = await _refreshTokens.ReplaceOneAsync(t => t.Id == token.Id, token);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteRefreshTokensByUserIdAsync(Guid userId)
    {
        var result = await _refreshTokens.DeleteManyAsync(t => t.UserId == userId);
        return result.DeletedCount > 0;
    }

    public async Task<PasswordResetToken> CreatePasswordResetTokenAsync(PasswordResetToken token)
    {
        await _passwordResetTokens.InsertOneAsync(token);
        return token;
    }

    public async Task<PasswordResetToken?> GetPasswordResetTokenAsync(string token)
    {
        return await _passwordResetTokens.Find(t => t.Token == token).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdatePasswordResetTokenAsync(PasswordResetToken token)
    {
        var result = await _passwordResetTokens.ReplaceOneAsync(t => t.Id == token.Id, token);
        return result.ModifiedCount > 0;
    }
}
