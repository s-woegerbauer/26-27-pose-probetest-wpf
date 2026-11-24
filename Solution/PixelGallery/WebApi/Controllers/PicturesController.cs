namespace WebApi.Controllers;

using Core.Contracts;
using Core.Entities;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// REST controller for pictures.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PicturesController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    /// <summary>
    /// Constructor.
    /// </summary>
    public PicturesController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    #region Dto

    /// <summary>
    /// Number of pixels of one color in a picture.
    /// </summary>
    public record ColorUsageDto(string Code, string Name, string Hex, int Pixels);

    /// <summary>
    /// Details of a picture.
    /// </summary>
    public record PictureDetailDto(
        int                  Id,
        string               Name,
        string?              Description,
        DateOnly             CreatedOn,
        string?              Artist,
        int                  Width,
        int                  Height,
        IList<string>        Tags,
        IList<ColorUsageDto> Colors);

    #endregion

    /// <summary>
    /// Get the details of a picture incl. tags and color statistics.
    /// </summary>
    /// <param name="id">id of the picture</param>
    /// <response code="200">picture found</response>
    /// <response code="404">no picture with this id</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PictureDetailDto>> GetAsync(int id)
    {
        var picture = await _uow.PictureRepository.GetNoTrackingByIdAsync(id,
            nameof(Picture.Artist),
            nameof(Picture.Tags),
            $"{nameof(Picture.Strokes)}.{nameof(Stroke.PaletteColor)}");

        if (picture == null)
        {
            return NotFound();
        }

        var colors = picture.Strokes!
            .GroupBy(s => s.PaletteColor!.Code) // NoTracking: group by key, not by instance
            .Select(g => new ColorUsageDto(g.Key, g.First().PaletteColor!.Name, g.First().PaletteColor!.Hex, g.Sum(s => s.Length)))
            .OrderByDescending(c => c.Pixels)
            .ThenBy(c => c.Code)
            .ToList();

        return Ok(new PictureDetailDto(
            picture.Id,
            picture.Name,
            picture.Description,
            picture.CreatedOn,
            picture.Artist?.Name,
            picture.Width,
            picture.Height,
            picture.Tags!.Select(t => t.Name).OrderBy(t => t).ToList(),
            colors));
    }
}
