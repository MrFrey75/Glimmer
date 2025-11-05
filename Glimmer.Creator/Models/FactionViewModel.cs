using System.ComponentModel.DataAnnotations;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;

namespace Glimmer.Creator.Models;

public class FactionListViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public List<FactionCardViewModel> Factions { get; set; } = new();
}

public class FactionCardViewModel
{
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FactionTypeEnum FactionType { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateFactionViewModel
{
    public Guid UniverseId { get; set; }
    
    [Required(ErrorMessage = "Faction name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Faction type is required")]
    public FactionTypeEnum FactionType { get; set; } = FactionTypeEnum.Government;
    
    // Organizational Structure
    public LeadershipStructureEnum LeadershipStructure { get; set; } = LeadershipStructureEnum.Unknown;
    
    [StringLength(1000)]
    public string MembershipCriteria { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Hierarchy { get; set; } = string.Empty;
    
    // Goals and Objectives
    [StringLength(1000)]
    public string PrimaryGoals { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Motivations { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string KeyActivities { get; set; } = string.Empty;
    
    // Historical Background
    [StringLength(2000)]
    public string FoundingHistory { get; set; } = string.Empty;
    
    [StringLength(2000)]
    public string EvolutionOverTime { get; set; } = string.Empty;
    
    // Additional Information
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
}

public class EditFactionViewModel
{
    public Guid UniverseId { get; set; }
    public Guid Uuid { get; set; }
    
    [Required(ErrorMessage = "Faction name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Faction type is required")]
    public FactionTypeEnum FactionType { get; set; }
    
    // Organizational Structure
    public LeadershipStructureEnum LeadershipStructure { get; set; } = LeadershipStructureEnum.Unknown;
    
    [StringLength(1000)]
    public string MembershipCriteria { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Hierarchy { get; set; } = string.Empty;
    
    // Goals and Objectives
    [StringLength(1000)]
    public string PrimaryGoals { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string Motivations { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string KeyActivities { get; set; } = string.Empty;
    
    // Historical Background
    [StringLength(2000)]
    public string FoundingHistory { get; set; } = string.Empty;
    
    [StringLength(2000)]
    public string EvolutionOverTime { get; set; } = string.Empty;
    
    // Additional Information
    [StringLength(2000)]
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class FactionDetailsViewModel
{
    public Guid UniverseId { get; set; }
    public string UniverseName { get; set; } = string.Empty;
    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public FactionTypeEnum FactionType { get; set; }
    
    // Organizational Structure
    public LeadershipStructureEnum LeadershipStructure { get; set; }
    public string MembershipCriteria { get; set; } = string.Empty;
    public string Hierarchy { get; set; } = string.Empty;
    
    // Goals and Objectives
    public string PrimaryGoals { get; set; } = string.Empty;
    public string Motivations { get; set; } = string.Empty;
    public string KeyActivities { get; set; } = string.Empty;
    
    // Historical Background
    public string FoundingHistory { get; set; } = string.Empty;
    public string EvolutionOverTime { get; set; } = string.Empty;
    
    // Additional Information
    public string AdditionalNotes { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
