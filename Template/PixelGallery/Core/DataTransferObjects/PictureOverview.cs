namespace Core.DataTransferObjects;

/// <summary>
/// One line of the picture overview (main window).
/// </summary>
public record PictureOverview(
    int      Id,
    DateOnly CreatedOn,
    string   Name,
    int?     ArtistId,
    string?  ArtistName,
    int      Width,
    int      Height,
    int      PixelCount,
    string   Tags)
{
    public string Size => $"{Width}x{Height}";
}
