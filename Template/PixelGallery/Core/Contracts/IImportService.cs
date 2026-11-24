namespace Core.Contracts;

public interface IImportService
{
    /// <summary>
    /// Imports the palette (Code;Name;Hex) from a csv file.
    /// </summary>
    Task<int> ImportPaletteAsync(string fileName);

    /// <summary>
    /// Imports all pictures of all sub folders (depth 1) of root.
    /// Returns the number of successfully imported pictures.
    /// </summary>
    Task<int> ImportFolderAsync(string root);

    /// <summary>
    /// Imports a single picture file.
    /// </summary>
    Task ImportPictureAsync(string name, string? description, string? artistCode, DateOnly createdOn, string fileName);
}
