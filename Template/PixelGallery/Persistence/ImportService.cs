namespace Persistence;

using System.Globalization;

using Core.Contracts;
using Core.Entities;
using Core.Pixel;

public class ImportService : IImportService
{
    private readonly IUnitOfWork _uow;

    public ImportService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> ImportPaletteAsync(string fileName)
    {
        var colors = (await File.ReadAllLinesAsync(fileName))
            .Skip(1) // header
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split(';'))
            .Select(parts => new PaletteColor { Code = parts[0], Name = parts[1], Hex = parts[2] })
            .ToList();

        using var trans = _uow.BeginTransaction();
        await _uow.PaletteColorRepository.AddRangeAsync(colors);
        await trans.CommitTransactionAsync();
        return colors.Count;
    }

    public async Task<int> ImportFolderAsync(string root)
    {
        int count = 0;
        foreach (var dir in Directory.GetDirectories(root))
        {
            foreach (var file in Directory.GetFiles(dir, "*.txt"))
            {
                var name  = Path.GetFileNameWithoutExtension(file);
                var parts = name.Split('_');
                var createdOn = parts.Length > 1 &&
                                DateOnly.TryParseExact(parts[^1], "yyyyMMdd", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out var date)
                    ? date
                    : DateOnly.FromDateTime(DateTime.Today);

                try
                {
                    await ImportPictureAsync(name, Path.GetRelativePath(root, file), Path.GetFileName(dir), createdOn, file);
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  skipped {Path.GetRelativePath(root, file)}: {ex.GetBaseException().Message}");
                }
            }
        }

        return count;
    }

    public async Task ImportPictureAsync(string name, string? description, string? artistCode, DateOnly createdOn, string fileName)
    {
        // TODO (Task 2):
        //  - parse the file with PixelParser
        //  - one transaction per picture
        //  - picture names are unique => InvalidOperationException
        //  - unknown color code => InvalidOperationException
        //  - artist and tags: use existing ones or create new ones (see GetOrCreateArtistAsync)
        //  - use the navigation properties: only ONE AddAsync for the picture
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    private async Task<Artist> GetOrCreateArtistAsync(string code)
    {
        var artist = (await _uow.ArtistRepository.GetAsync(a => a.Code == code)).FirstOrDefault();
        if (artist == null)
        {
            artist = new Artist { Code = code, Name = code };
            await _uow.ArtistRepository.AddAsync(artist);
        }

        return artist;
    }

}
