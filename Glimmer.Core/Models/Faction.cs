using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Faction : BaseEntity
{
    public FactionTypeEnum FactionType { get; set; } = FactionTypeEnum.Unknown;

    // Organizational Structure
    public LeadershipStructureEnum LeadershipStructure { get; set; } = LeadershipStructureEnum.Unknown;
    public string? MembershipCriteria { get; set; }
    public string? Hierarchy { get; set; }
    // Goals and Objectives
    public string? PrimaryGoals { get; set; }
    public string? Motivations { get; set; }
    public string? KeyActivities { get; set; }
    // Historical Background
    public string? FoundingHistory { get; set; }
    public string? EvolutionOverTime { get; set; }
    // Additional Information
    public string? AdditionalNotes { get; set; }
}

public enum LeadershipStructureEnum
{
    Unknown = 0,
    SingleLeader = 1,
    Council = 2,
    Democratic = 3,
    Hierarchical = 4,
    Collective = 5
}