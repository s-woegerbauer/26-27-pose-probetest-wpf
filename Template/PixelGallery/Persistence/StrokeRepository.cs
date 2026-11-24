namespace Persistence;

using Base.Persistence;

using Core.Contracts;
using Core.Entities;

public class StrokeRepository : GenericRepository<Stroke>, IStrokeRepository
{
    public StrokeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
