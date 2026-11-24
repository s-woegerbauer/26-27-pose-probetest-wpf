namespace Persistence.Mapping;

using Base.Persistence.Mappings;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class ArtistMapping
{
    public static void Map(this EntityTypeBuilder<Artist> entity)
    {
        entity.ToTable("Artist");
        entity.HasKey(a => a.Id);

        entity.Property(a => a.Code).AsRequiredText(16);
        entity.HasIndex(a => a.Code).IsUnique();

        entity.Property(a => a.Name).AsRequiredText(128);
    }
}
