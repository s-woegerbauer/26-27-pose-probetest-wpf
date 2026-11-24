namespace Persistence.Mapping;

using Base.Persistence.Mappings;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class TagMapping
{
    public static void Map(this EntityTypeBuilder<Tag> entity)
    {
        entity.ToTable("Tag");
        entity.HasKey(t => t.Id);

        entity.Property(t => t.Name).AsRequiredText(64);
        entity.HasIndex(t => t.Name).IsUnique();

        entity.HasMany(t => t.Pictures)
            .WithMany(p => p.Tags)
            .UsingEntity(j => j.ToTable("PictureTag"));
    }
}
