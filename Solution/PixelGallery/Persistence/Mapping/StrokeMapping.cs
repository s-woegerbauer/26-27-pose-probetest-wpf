namespace Persistence.Mapping;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class StrokeMapping
{
    public static void Map(this EntityTypeBuilder<Stroke> entity)
    {
        entity.ToTable("Stroke");
        entity.HasKey(s => s.Id);

        entity.HasIndex(s => new { s.PictureId, s.Row, s.Column }).IsUnique();

        entity.HasOne(s => s.Picture)
            .WithMany(p => p.Strokes)
            .HasForeignKey(s => s.PictureId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(s => s.PaletteColor)
            .WithMany(c => c.Strokes)
            .HasForeignKey(s => s.PaletteColorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
