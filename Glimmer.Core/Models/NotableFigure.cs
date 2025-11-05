using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class NotableFigure : BaseEntity
{


    public NotableFigure()
    {
        // Initialize with default values - Species-specific defaults will be applied when Species is set
        InitializeDefaults();
    }

    /// <summary>
    /// Applies default values from the assigned Species to empty properties.
    /// Should be called when Species property is set.
    /// </summary>
    public void ApplySpeciesDefaults()
    {
        if (Species == null) return;

        Height = string.IsNullOrEmpty(Height) ? Species.AverageHeight : Height;
        Weight = string.IsNullOrEmpty(Weight) ? Species.AverageWeight : Weight;
        EyeColor = string.IsNullOrEmpty(EyeColor) ? Species.EyeColor : EyeColor;
        HairColor = string.IsNullOrEmpty(HairColor) ? Species.HairColor : HairColor;
        SkinColor = string.IsNullOrEmpty(SkinColor) ? Species.SkinColor : SkinColor;
        DistinguishingFeatures = string.IsNullOrEmpty(DistinguishingFeatures) ? Species.DistinguishingFeatures : DistinguishingFeatures;
        MagicalAbilitiesDescription = string.IsNullOrEmpty(MagicalAbilitiesDescription) ? Species.MagicalAbilitiesDescription : MagicalAbilitiesDescription;
    }

    /// <summary>
    /// Initializes default values for the NotableFigure properties.
    /// </summary>
    private void InitializeDefaults()
    {
        Height = string.Empty;
        Weight = string.Empty;
        EyeColor = string.Empty;
        HairColor = string.Empty;
        SkinColor = string.Empty;
        DistinguishingFeatures = string.Empty;
        MagicalAbilitiesDescription = string.Empty;
        Occupation = string.Empty;
        NotableAchievements = string.Empty;
        Biography = string.Empty;
        AdditionalNotes = string.Empty;
    }

    public FigureTypeEnum FigureType { get; set; } = FigureTypeEnum.Unknown;
    
    private Species? _species;
    public Species? Species 
    { 
        get => _species;
        set 
        { 
            _species = value;
            if (_species != null)
            {
                ApplySpeciesDefaults();
            }
        }
    }

    //physical characteristics
    public string Height { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;

    public HeightMeasureEnum HeightMeasure { get; set; } = HeightMeasureEnum.Unknown;
    public WeightMeasureEnum WeightMeasure { get; set; } = WeightMeasureEnum.Unknown;

    public string EyeColor { get; set; } = string.Empty;
    public string HairColor { get; set; } = string.Empty;
    public string SkinColor { get; set; } = string.Empty;
    public string DistinguishingFeatures { get; set; } = string.Empty;
    public GenderEnum Gender { get; set; } = GenderEnum.Unknown;
    public SexualOrientationEnum SexualOrientation { get; set; } = SexualOrientationEnum.Unknown;

       // Magical or Special Abilities
    public bool HasMagicalAbilities { get; set; } = false;
    public string MagicalAbilitiesDescription { get; set; } = string.Empty;

    //biographical information
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }

    //additional information
    public string Occupation { get; set; } = string.Empty;
    public string NotableAchievements { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;

    public string AdditionalNotes { get; set; } = string.Empty;

}
