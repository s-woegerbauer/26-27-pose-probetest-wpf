namespace Persistence;

using Core.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Registers DbContext, UnitOfWork, all repositories and the ImportService.
    /// </summary>
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        return services
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddTransient<IArtistRepository, ArtistRepository>()
            .AddTransient<IPictureRepository, PictureRepository>()
            .AddTransient<IStrokeRepository, StrokeRepository>()
            .AddTransient<IPaletteColorRepository, PaletteColorRepository>()
            .AddTransient<ITagRepository, TagRepository>()
            .AddTransient<IImportService, ImportService>();
    }
}
