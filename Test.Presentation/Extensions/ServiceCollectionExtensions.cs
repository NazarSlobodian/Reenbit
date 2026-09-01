using Test.Application.Common;
using Test.Application.Interfaces;
using Test.Application.Interfaces.Repositories;
using Test.Application.Interfaces.Services;
using Test.Application.Services;
using Test.Infrastructure.Identity;
using Test.Infrastructure.Notifications;
using Test.Infrastructure.Persistence;
using Test.Infrastructure.Persistence.Repositories;

namespace Test.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomManagementService, RoomManagementService>();
            services.AddScoped<IBookingManagementService, BookingManagementService>();
            services.AddScoped<ITimeSlotGenerator, TimeSlotGenerator>();
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            return services;
        }
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddScoped<ITimeSlotNotifier, SignalRTimeSlotNotifier>();

            return services;
        }

        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SlotGenerationOptions>(configuration.GetSection("SlotGeneration"));
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
