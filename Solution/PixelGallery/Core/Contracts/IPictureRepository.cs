namespace Core.Contracts;

using Base.Core.Contracts;

using Core.DataTransferObjects;
using Core.Entities;

public interface IPictureRepository : IGenericRepository<Picture>
{
    /// <summary>
    /// Returns the overview of all pictures, filtered by artist and/or tag,
    /// sorted by CreatedOn descending (newest first), then by Name.
    /// Filtering is done by the database.
    /// </summary>
    /// <param name="artistId">null => pictures of all artists</param>
    /// <param name="tag">null/empty => no tag filter, otherwise pictures with a tag that contains the text</param>
    Task<IList<PictureOverview>> GetOverviewAsync(int? artistId, string? tag);
}
