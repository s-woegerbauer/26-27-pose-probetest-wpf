namespace Persistence;

using Base.Persistence;

using Core.Contracts;
using Core.Entities;

public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
{
    public ArtistRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
