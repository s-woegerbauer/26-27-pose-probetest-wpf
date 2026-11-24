namespace Persistence;

using Base.Persistence;

using Core.Contracts;
using Core.DataTransferObjects;
using Core.Entities;

using Microsoft.EntityFrameworkCore;

public class PictureRepository : GenericRepository<Picture>, IPictureRepository
{
    public PictureRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<PictureOverview>> GetOverviewAsync(int? artistId, string? tag)
    {
        var query = DbSet.AsNoTracking();
        if (artistId != null)
        {
            query = query.Where(p => p.ArtistId == artistId);
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query.Where(p => p.Tags!.Any(t => t.Name.Contains(tag)));
        }

        return await query
            .OrderByDescending(p => p.CreatedOn)
            .ThenBy(p => p.Name)
            .Select(p => new PictureOverview(
                p.Id,
                p.CreatedOn,
                p.Name,
                p.ArtistId,
                p.Artist!.Name,
                p.Width,
                p.Height,
                p.Strokes!.Sum(s => s.Length),
                string.Join(",", p.Tags!.OrderBy(t => t.Name).Select(t => t.Name))))
            .ToListAsync();
    }
}
