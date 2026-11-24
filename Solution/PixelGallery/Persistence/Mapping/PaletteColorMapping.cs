namespace Persistence.Mapping;

using Base.Persistence.Mappings;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class PaletteColorMapping
{
    public static void Map(this EntityTypeBuilder<PaletteColor> entity)
    {
        entity.ToTable("PaletteColor");
        entity.HasKey(c => c.Id);

        entity.Property(c => c.Code).AsRequiredText(1);
        entity.HasIndex(c => c.Code).IsUnique();

        entity.Property(c => c.Name).AsRequiredText(64);
        entity.Property(c => c.Hex).AsRequiredText(7);
    }
}
