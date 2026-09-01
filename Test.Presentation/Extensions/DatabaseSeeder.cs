using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using Test.Application.DTOs.RoomManagement;
using Test.Application.Interfaces.Services;
using Test.Domain.Constants;
using Test.Infrastructure.Identity;
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

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in new[] { AppRoles.Admin, AppRoles.User })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            const string adminEmail = "admin@reenbit.dev";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "Seeded Admin" };
                await userManager.CreateAsync(admin, "Admin@12345");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
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