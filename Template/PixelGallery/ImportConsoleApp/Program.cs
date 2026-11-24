using Base.Core;
using Base.Tools;

using Core.Contracts;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Persistence;

ConfigureDependencyInjector();
await RecreateDatabaseAsync();
await ImportAsync();

void ConfigureDependencyInjector()
{
    var configuration    = ConfigurationHelper.GetConfiguration();
    var connectionString = configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    AppService.ServiceCollection = new ServiceCollection();
    AppService.ServiceCollection
        .AddSingleton(configuration)
        .AddPersistence(connectionString);

    AppService.BuildServiceProvider();
}

async Task RecreateDatabaseAsync()
{
    Console.WriteLine("=====================");
    using var scope = AppService.ServiceProvider!.CreateScope();
    var       uow   = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    Console.WriteLine("Deleting database ...");
    await uow.DeleteDatabaseAsync();

    Console.WriteLine("Creating database ...");
    await uow.CreateDatabaseAsync(); // EnsureCreated: no migrations necessary
}

async Task ImportAsync()
{
    Console.WriteLine("=====================");
    var importData = Path.Combine(AppContext.BaseDirectory, "ImportData");

    using (var scope = AppService.ServiceProvider!.CreateScope())
    {
        var import = scope.ServiceProvider.GetRequiredService<IImportService>();
        Console.WriteLine("Import palette ...");
        var colors = await import.ImportPaletteAsync(Path.Combine(importData, "palette.csv"));
        Console.WriteLine($"  {colors} colors");
    }

    using (var scope = AppService.ServiceProvider!.CreateScope())
    {
        var import = scope.ServiceProvider.GetRequiredService<IImportService>();
        Console.WriteLine("Import pictures ...");
        try
        {
            var count = await import.ImportFolderAsync(importData);
            Console.WriteLine($"Import done: {count} pictures");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetBaseException().Message);
        }
    }
}
