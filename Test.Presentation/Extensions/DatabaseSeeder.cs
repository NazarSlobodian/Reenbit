using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Entities;
using Test.Infrastructure.Persistence;

namespace Test.Presentation.Extensions
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var maxRetries = 5;
            for (int retry = 1; retry <= maxRetries; retry++)
            {
                try
                {
                    context.Database.Migrate();
                    break;
                }
                catch (SqlException)
                {
                    if (retry == maxRetries) throw;
                    Console.WriteLine($"Database not ready yet. Retrying {retry}/{maxRetries} in 3 seconds...");
                    Thread.Sleep(3000);
                }
            }

            if (!context.Rooms.Any())
            {
                var roomA = new Room { Name = "Зал А", Capacity = 50, BasePricePerHour = 2000 };
                var roomB = new Room { Name = "Зал B", Capacity = 100, BasePricePerHour = 3500 };
                var roomC = new Room { Name = "Зал C", Capacity = 30, BasePricePerHour = 1500 };

                roomA.Services.Add(new RoomService { Name = "Проєктор", Price = 500 });
                roomA.Services.Add(new RoomService { Name = "Wi-Fi", Price = 300 });

                roomB.Services.Add(new RoomService { Name = "Звук", Price = 700 });
                roomB.Services.Add(new RoomService { Name = "Wi-Fi", Price = 300 });
                roomB.Services.Add(new RoomService { Name = "Проєктор", Price = 500 });

                roomC.Services.Add(new RoomService { Name = "Wi-Fi", Price = 300 });

                context.Rooms.AddRange(roomA, roomB, roomC);
                context.SaveChanges();
            }
        }
    }
}
