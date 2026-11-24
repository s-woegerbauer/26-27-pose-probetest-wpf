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

        // TODO (Task 1): unique index and relations (Picture, PaletteColor) according to the ERD
    }
}
