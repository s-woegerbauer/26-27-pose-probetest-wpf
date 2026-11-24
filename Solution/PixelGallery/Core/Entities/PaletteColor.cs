namespace Core.Entities;

using Base.Core.Entities;

public class PaletteColor : EntityObject
{
    /// <summary>
    /// Single upper case letter used in the picture files (e.g. "R").
    /// </summary>
    public required string Code { get; set; }

    public required string Name { get; set; }

    /// <summary>
    /// Color as hex string, e.g. "#E53935".
    /// </summary>
    public required string Hex { get; set; }

    public IList<Stroke>? Strokes { get; set; }
}
