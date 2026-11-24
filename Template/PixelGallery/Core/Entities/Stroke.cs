namespace Core.Entities;

using Base.Core.Entities;

public class Stroke : EntityObject
{
    public int Row    { get; set; }
    public int Column { get; set; }
    public int Length { get; set; }

    public int      PictureId { get; set; }
    public Picture? Picture   { get; set; }

    public int           PaletteColorId { get; set; }
    public PaletteColor? PaletteColor   { get; set; }
}
