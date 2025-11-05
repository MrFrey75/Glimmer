using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class NotableFigure : BaseEntity
{
    public FigureTypeEnum FigureType { get; set; } = FigureTypeEnum.Unknown;
    public SpeciesTypeEnum SpeciesType { get; set; } = SpeciesTypeEnum.Human;

    //physical characteristics
    public string? Height { get; set; }
    public string? Weight { get; set; }
    public string? EyeColor { get; set; }
    public string? HairColor { get; set; }
    public string? SkinColor { get; set; }
    public string? DistinguishingFeatures { get; set; }
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public SexualOrientationEnum SexualOrientation { get; set; } = SexualOrientationEnum.Unknown;

       // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    public string? MagicalAbilitiesDescription { get; set; }

    //biographical information
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }

    //additional information
    public string? Occupation { get; set; }
    public string? NotableAchievements { get; set; }
    public string? Biography { get; set; }

}
