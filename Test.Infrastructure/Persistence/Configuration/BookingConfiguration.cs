using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Test.Domain.Entities;
using Test.Infrastructure.Identity;

namespace Test.Infrastructure.Persistence.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.TimeSlot)
                   .WithOne(x => x.Booking)
                   .HasForeignKey<Booking>(x => x.TimeSlotId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TimeSlotId).IsUnique();
        }
    }
}