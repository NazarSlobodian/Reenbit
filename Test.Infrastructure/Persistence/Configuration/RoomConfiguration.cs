using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Configuration
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
            builder.Property(x => x.BasePricePerHour).HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.Property(r => r.RowVersion).IsRowVersion();
        }
    }
}
