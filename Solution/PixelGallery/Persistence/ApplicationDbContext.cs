namespace Persistence;

using System.Diagnostics;

using Base.Tools;

using Core.Entities;

using Microsoft.EntityFrameworkCore;

using Persistence.Mapping;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = ConfigurationHelper.GetConfiguration().Get("DefaultConnection", "ConnectionStrings");
            optionsBuilder.UseSqlServer(connectionString);
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>().Map();
        modelBuilder.Entity<Picture>().Map();
        modelBuilder.Entity<PaletteColor>().Map();
        modelBuilder.Entity<Stroke>().Map();
        modelBuilder.Entity<Tag>().Map();
    }
}
