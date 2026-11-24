namespace Persistence.Mapping;

using Base.Persistence.Mappings;

using Core.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class TagMapping
{
    public static void Map(this EntityTypeBuilder<Tag> entity)
    {
        // TODO (Task 1): table, key, properties, unique index and m:n relation to Picture
        //                (join table "PictureTag") according to the ERD
    }
}
