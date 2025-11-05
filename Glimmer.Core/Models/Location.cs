using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Location : BaseEntity
{
    public Location? ParentLocation { get; set; } = null;
    public LocationTypeEnum LocationType { get; set; } = LocationTypeEnum.Unknown;

    // Geographical Information
    public string? Coordinates { get; set; }
    public string? Climate { get; set; }
    public string? Terrain { get; set; }
    public string? NaturalResources { get; set; }

    // Address and Political Information
    public string? Address { get; set; }
    public string? PoliticalAffiliation { get; set; }

    // Cultural and Societal Information
    public string? Inhabitants { get; set; }
    public string? LanguagesSpoken { get; set; }
    public string? MajorCitiesOrSettlements { get; set; }
    public string? HistoricalSignificance { get; set; }
    public string? NotableLandmarks { get; set; }
    public string? CurrentEvents { get; set; }

    // Additional Information
    public string? AdditionalNotes { get; set; }
}
