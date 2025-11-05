using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Location : BaseEntity
{
    public Location? ParentLocation { get; set; } = null;
    public LocationTypeEnum LocationType { get; set; } = LocationTypeEnum.Unknown;

    // Geographical Information
    public string Coordinates { get; set; } = string.Empty;
    public ClimateEnum Climate { get; set; } = ClimateEnum.Unknown;
    public TerrainEnum Terrain { get; set; } = TerrainEnum.Unknown;
    public string NaturalResources { get; set; } = string.Empty;

    // Address and Political Information
    public string Address { get; set; } = string.Empty;
    public string PoliticalAffiliation { get; set; } = string.Empty;

    // Cultural and Societal Information
    public string Inhabitants { get; set; } = string.Empty;
    public string LanguagesSpoken { get; set; } = string.Empty;
    public string HistoricalSignificance { get; set; } = string.Empty;

    // Additional Information
    public string AdditionalNotes { get; set; } = string.Empty;

}
