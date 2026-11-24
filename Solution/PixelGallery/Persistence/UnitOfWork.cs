namespace Persistence;

using Base.Persistence;

using Core.Contracts;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public IArtistRepository       ArtistRepository       { get; }
    public IPictureRepository      PictureRepository      { get; }
    public IStrokeRepository       StrokeRepository       { get; }
    public IPaletteColorRepository PaletteColorRepository { get; }
    public ITagRepository          TagRepository          { get; }

    public UnitOfWork(ApplicationDbContext context,
        IArtistRepository                  artistRepository,
        IPictureRepository                 pictureRepository,
        IStrokeRepository                  strokeRepository,
        IPaletteColorRepository            paletteColorRepository,
        ITagRepository                     tagRepository
    ) : base(context)
    {
        ArtistRepository       = artistRepository;
        PictureRepository      = pictureRepository;
        StrokeRepository       = strokeRepository;
        PaletteColorRepository = paletteColorRepository;
        TagRepository          = tagRepository;
    }
}
