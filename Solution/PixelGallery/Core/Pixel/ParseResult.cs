namespace Core.Pixel;

/// <summary>
/// Result of parsing a picture file.
/// </summary>
/// <param name="Tags">tags of the #tags: header line</param>
/// <param name="Runs">all colored runs (transparent pixels are not included)</param>
/// <param name="Width">max. number of columns of all rows</param>
/// <param name="Height">number of rows</param>
public record ParseResult(IList<string> Tags, IList<Run> Runs, int Width, int Height);
