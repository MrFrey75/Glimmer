using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Fact Operations
/// </summary>
public partial class EntityService
{
public async Task<Fact?> CreateFactAsync(Guid universeId, string name, string description, string value, FactTypeEnum factType)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        var fact = new Fact
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            Value = value,
            FactType = factType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Facts.Add(fact);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return fact;
    }

    public async Task<Fact?> GetFactByIdAsync(Guid universeId, Guid factId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Facts.FirstOrDefault(f => f.Uuid == factId && !f.IsDeleted);
    }

    public async Task<List<Fact>> GetFactsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Facts.Where(f => !f.IsDeleted).ToList() ?? new List<Fact>();
    }

    public async Task<bool> UpdateFactAsync(Guid universeId, Fact fact)
    {

        var existing = await GetFactByIdAsync(universeId, fact.Uuid);
        if (existing == null) return false;

        existing.Name = fact.Name;
        existing.Description = fact.Description;
        existing.Value = fact.Value;
        existing.FactType = fact.FactType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteFactAsync(Guid universeId, Guid factId)
    {

        var fact = await GetFactByIdAsync(universeId, factId);
        if (fact == null) return false;

        fact.IsDeleted = true;
        fact.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
