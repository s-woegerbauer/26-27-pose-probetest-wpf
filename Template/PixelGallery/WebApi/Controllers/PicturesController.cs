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

    // TODO (Task 4): GET /Pictures/{id}
    //   200 => PictureDetailDto (Tags sorted by name, Colors sorted by Pixels descending)
    //   404 => no picture with this id
}
