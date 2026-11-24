namespace Core.Entities;

using Base.Core.Entities;

public class Tag : EntityObject
{
    public required string Name { get; set; }

    public IList<Picture>? Pictures { get; set; }
}
