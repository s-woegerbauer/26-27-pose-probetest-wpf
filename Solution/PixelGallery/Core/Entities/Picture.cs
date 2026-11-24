namespace Core.Entities;

using Base.Core.Entities;

public class Picture : EntityObject
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateOnly CreatedOn { get; set; }

    public int Width  { get; set; }
    public int Height { get; set; }

    public int?    ArtistId { get; set; }
    public Artist? Artist   { get; set; }

    public IList<Stroke>? Strokes { get; set; }

    public IList<Tag>? Tags { get; set; }
}
