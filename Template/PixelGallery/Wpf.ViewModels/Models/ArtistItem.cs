namespace Wpf.ViewModels.Models;

/// <summary>
/// Entry of an artist combobox. Id == null is used for "&lt;All&gt;" (filter) or "(none)" (edit).
/// </summary>
public record ArtistItem(int? Id, string Name);
