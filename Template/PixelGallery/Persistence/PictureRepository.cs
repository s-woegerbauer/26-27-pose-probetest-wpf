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
        // TODO (Task 3): filter (artist, tag), sort and project to PictureOverview - all done by the database!
        await Task.CompletedTask;
        return new List<PictureOverview>();
    }
}
