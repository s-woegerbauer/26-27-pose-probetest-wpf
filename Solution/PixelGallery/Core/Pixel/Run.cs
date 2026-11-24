namespace Core.Pixel;

/// <summary>
/// A horizontal run of pixels with the same color.
/// </summary>
/// <param name="Row">0-based row</param>
/// <param name="Column">0-based start column</param>
/// <param name="Length">number of pixels</param>
/// <param name="ColorCode">palette code, e.g. 'R'</param>
public record Run(int Row, int Column, int Length, char ColorCode);
