namespace Core.Contracts;

using Base.Core.Contracts;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IArtistRepository       ArtistRepository       { get; }
    IPictureRepository      PictureRepository      { get; }
    IStrokeRepository       StrokeRepository       { get; }
    IPaletteColorRepository PaletteColorRepository { get; }
    ITagRepository          TagRepository          { get; }
}
