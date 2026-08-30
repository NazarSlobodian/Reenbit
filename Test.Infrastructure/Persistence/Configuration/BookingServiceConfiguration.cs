using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Test.Domain.Entities;

namespace Test.Infrastructure.Persistence.Configuration
{
    public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
    {
        public void Configure(EntityTypeBuilder<BookingService> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ServiceName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PriceAtBooking).HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Booking)
                   .WithMany(x => x.BookingServices)
                   .HasForeignKey(x => x.BookingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.RoomService)
                   .WithMany()
                   .HasForeignKey(x => x.RoomServiceId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
