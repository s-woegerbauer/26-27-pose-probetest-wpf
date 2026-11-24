namespace Wpf.ViewModels.Models;

/// <summary>
/// One entry of the color legend in the preview window.
/// </summary>
/// <param name="Name">name of the palette color, e.g. "Rot"</param>
/// <param name="Hex">color as hex string, e.g. "#E53935" (can be bound to Background/Fill)</param>
/// <param name="PixelCount">number of pixels with this color</param>
public record ColorUsage(string Name, string Hex, int PixelCount);
