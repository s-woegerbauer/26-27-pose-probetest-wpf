namespace Core.Entities;

using Base.Core.Entities;

public class Artist : EntityObject
{
    public required string Code { get; set; }

    public required string Name { get; set; }

    public IList<Picture>? Pictures { get; set; }
}
