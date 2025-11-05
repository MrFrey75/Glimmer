using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Faction Operations
/// </summary>
public partial class EntityService
{
public async Task<Faction?> CreateFactionAsync(Guid universeId, Faction faction)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null) return null;

        faction.Uuid = Guid.NewGuid();
        faction.CreatedAt = DateTime.UtcNow;
        faction.UpdatedAt = DateTime.UtcNow;

        universe.Factions.Add(faction);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        return faction;
    }

    public async Task<Faction?> GetFactionByIdAsync(Guid universeId, Guid factionId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Factions.FirstOrDefault(f => f.Uuid == factionId && !f.IsDeleted);
    }

    public async Task<List<Faction>> GetFactionsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Factions.Where(f => !f.IsDeleted).ToList() ?? new List<Faction>();
    }

    public async Task<bool> UpdateFactionAsync(Guid universeId, Faction faction)
    {

        var existing = await GetFactionByIdAsync(universeId, faction.Uuid);
        if (existing == null) return false;

        existing.Name = faction.Name;
        existing.Description = faction.Description;
        existing.FactionType = faction.FactionType;
        existing.LeadershipStructure = faction.LeadershipStructure;
        existing.MembershipCriteria = faction.MembershipCriteria;
        existing.Hierarchy = faction.Hierarchy;
        existing.PrimaryGoals = faction.PrimaryGoals;
        existing.Motivations = faction.Motivations;
        existing.KeyActivities = faction.KeyActivities;
        existing.FoundingHistory = faction.FoundingHistory;
        existing.EvolutionOverTime = faction.EvolutionOverTime;
        existing.AdditionalNotes = faction.AdditionalNotes;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null)
        {
            universe.UpdatedAt = DateTime.UtcNow;
            await _universeRepository.UpdateAsync(universe);
        }

        return true;
    }

    public async Task<bool> DeleteFactionAsync(Guid universeId, Guid factionId)
    {

        var faction = await GetFactionByIdAsync(universeId, factionId);
        if (faction == null) return false;

        faction.IsDeleted = true;
        faction.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
