using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;
using Test.Application.Services;

using Test.Infrastructure.Persistence.Repositories;

namespace Test.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomManagementService, RoomManagementService>();
            services.AddScoped<IBookingManagementService, BookingManagementService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            return services;
        }
    }
}
