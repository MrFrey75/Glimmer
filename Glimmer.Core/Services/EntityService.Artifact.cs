using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Artifact Operations
/// </summary>
public partial class EntityService
{
public async Task<Artifact?> CreateArtifactAsync(Guid universeId, string name, string description, ArtifactTypeEnum artifactType)
    {
        _logger.LogDebug("Creating artifact '{Name}' in universe {UniverseId}", name, universeId);

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe == null)
        {
            _logger.LogWarning("Artifact creation failed: Universe {UniverseId} not found", universeId);
            return null;
        }

        var artifact = new Artifact
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            ArtifactType = artifactType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        universe.Artifacts.Add(artifact);
        universe.UpdatedAt = DateTime.UtcNow;
        await _universeRepository.UpdateAsync(universe);
        
        _logger.LogInformation("Artifact {ArtifactId} '{Name}' created in universe {UniverseId}", 
            artifact.Uuid, name, universeId);
        
        return artifact;
    }

    public async Task<Artifact?> GetArtifactByIdAsync(Guid universeId, Guid artifactId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Artifacts.FirstOrDefault(a => a.Uuid == artifactId && !a.IsDeleted);
    }

    public async Task<List<Artifact>> GetArtifactsByUniverseAsync(Guid universeId)
    {

        var universe = await GetUniverseByIdAsync(universeId);
        return universe?.Artifacts.Where(a => !a.IsDeleted).ToList() ?? new List<Artifact>();
    }

    public async Task<bool> UpdateArtifactAsync(Guid universeId, Artifact artifact)
    {

        var existing = await GetArtifactByIdAsync(universeId, artifact.Uuid);
        if (existing == null) return false;

        existing.Name = artifact.Name;
        existing.Description = artifact.Description;
        existing.ArtifactType = artifact.ArtifactType;
        existing.UpdatedAt = DateTime.UtcNow;

        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;

        return true;
    }

    public async Task<bool> DeleteArtifactAsync(Guid universeId, Guid artifactId)
    {

        var artifact = await GetArtifactByIdAsync(universeId, artifactId);
        if (artifact == null) return false;

        artifact.IsDeleted = true;
        artifact.UpdatedAt = DateTime.UtcNow;
        var universe = await GetUniverseByIdAsync(universeId);
        if (universe != null) universe.UpdatedAt = DateTime.UtcNow;
        if (universe != null) await _universeRepository.UpdateAsync(universe);
        return true;
    }
}
