namespace Persistence.Mapping;

using Base.Persistence.Mappings;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class PictureMapping
{
    public static void Map(this EntityTypeBuilder<Picture> entity)
    {
        entity.ToTable("Picture");
        entity.HasKey(p => p.Id);

        entity.Property(p => p.Name).AsRequiredText(256);
        entity.HasIndex(p => p.Name).IsUnique();

        entity.Property(p => p.Description).HasMaxLength(2000);

        entity.HasOne(p => p.Artist)
            .WithMany(a => a.Pictures)
            .HasForeignKey(p => p.ArtistId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
