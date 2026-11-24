namespace Persistence;

using Base.Persistence;

using Core.Contracts;
using Core.Entities;

public class TagRepository : GenericRepository<Tag>, ITagRepository
{
    public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
