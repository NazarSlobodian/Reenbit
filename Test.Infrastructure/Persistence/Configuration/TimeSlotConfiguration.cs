using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Configuration
{
    public class TimeSlotConfiguration : IEntityTypeConfiguration<TimeSlot>
    {
        public void Configure(EntityTypeBuilder<TimeSlot> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RowVersion).IsRowVersion();

            builder.HasOne(x => x.Room)
                   .WithMany(x => x.TimeSlots)
                   .HasForeignKey(x => x.RoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.RoomId, x.StartTime }).IsUnique();

            builder.ToTable(tb => tb.HasCheckConstraint("CK_TimeSlot_EndAfterStart", "[EndTime] > [StartTime]"));

            builder.HasQueryFilter(x => !x.Room.IsDeleted);
        }
    }
}