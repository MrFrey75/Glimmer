using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Faction : BaseEntity
{
    public FactionTypeEnum FactionType { get; set; } = FactionTypeEnum.Unknown;

    // Organizational Structure
    public LeadershipStructureEnum LeadershipStructure { get; set; } = LeadershipStructureEnum.Unknown;
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
}

