using Glimmer.Core.Enums;

namespace Glimmer.Core.Models;

public class Artifact : BaseEntity
{
    public ArtifactTypeEnum ArtifactType { get; set; } = ArtifactTypeEnum.Unknown;
}
