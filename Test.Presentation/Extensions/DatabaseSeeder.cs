using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces.Services;
using Test.Infrastructure.Persistence;

namespace Test.Presentation.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var maxRetries = 5;
            for (int retry = 1; retry <= maxRetries; retry++)
            {
                try
                {
                    await context.Database.MigrateAsync();
                    break;
                }
                catch (SqlException)
                {
                    if (retry == maxRetries) throw;
                    Console.WriteLine($"Database not ready yet. Retrying {retry}/{maxRetries} in 3 seconds...");
                    await Task.Delay(3000);
                }
            }

            if (!context.Rooms.Any())
            {
                var roomService = scope.ServiceProvider.GetRequiredService<IRoomManagementService>();

                await roomService.CreateRoomAsync(new CreateRoomDto("Зал А"), CancellationToken.None);
                await roomService.CreateRoomAsync(new CreateRoomDto("Зал B"), CancellationToken.None);
                await roomService.CreateRoomAsync(new CreateRoomDto("Зал C"), CancellationToken.None);
            }
        }
    }
}