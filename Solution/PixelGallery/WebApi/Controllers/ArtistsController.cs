namespace WebApi.Controllers;

using Core.Contracts;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// REST controller for artists (example controller).
/// </summary>
[ApiController]
[Route("[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Constructor.
    /// </summary>
    public ArtistsController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    /// <summary>
    /// ArtistDto
    /// </summary>
    public record ArtistDto(int Id, string Code, string Name);

    /// <summary>
    /// Get all artists, sorted by name.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArtistDto>>> GetAsync()
    {
        var artists = await _uow.ArtistRepository.GetNoTrackingAsync(
            orderBy: q => q.OrderBy(a => a.Name));

        return Ok(artists.Select(a => new ArtistDto(a.Id, a.Code, a.Name)).ToList());
    }
}
