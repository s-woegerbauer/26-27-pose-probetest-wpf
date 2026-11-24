namespace Persistence;

using Base.Persistence;

using Core.Contracts;
using Core.Entities;

public class PaletteColorRepository : GenericRepository<PaletteColor>, IPaletteColorRepository
{
    public PaletteColorRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
