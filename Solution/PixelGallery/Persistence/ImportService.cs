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
        var result = PixelParser.Parse(await File.ReadAllTextAsync(fileName));

        using var trans = _uow.BeginTransaction();

        if (await _uow.PictureRepository.CountAsync(p => p.Name == name) > 0)
        {
            throw new InvalidOperationException($"Picture '{name}' already exists.");
        }

        var palette = (await _uow.PaletteColorRepository.GetAsync()).ToDictionary(c => c.Code[0]);
        var strokes = new List<Stroke>();
        foreach (var run in result.Runs)
        {
            if (!palette.TryGetValue(run.ColorCode, out var color))
            {
                throw new InvalidOperationException($"Unknown color code '{run.ColorCode}'.");
            }

            strokes.Add(new Stroke { Row = run.Row, Column = run.Column, Length = run.Length, PaletteColor = color });
        }

        var tags = new List<Tag>();
        foreach (var tagName in result.Tags.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            tags.Add(await GetOrCreateTagAsync(tagName));
        }

        var picture = new Picture
        {
            Name        = name,
            Description = description,
            CreatedOn   = createdOn,
            Width       = result.Width,
            Height      = result.Height,
            Artist      = string.IsNullOrWhiteSpace(artistCode) ? null : await GetOrCreateArtistAsync(artistCode),
            Strokes     = strokes,
            Tags        = tags,
        };

        await _uow.PictureRepository.AddAsync(picture); // ONE Add: strokes, tags and artist via navigation properties
        await trans.CommitTransactionAsync();
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

    private async Task<Tag> GetOrCreateTagAsync(string name)
    {
        var tag = (await _uow.TagRepository.GetAsync(t => t.Name == name)).FirstOrDefault();
        if (tag == null)
        {
            tag = new Tag { Name = name };
            await _uow.TagRepository.AddAsync(tag);
        }

        return tag;
    }
}
