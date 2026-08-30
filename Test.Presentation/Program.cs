using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using Test.Infrastructure.Persistence;
using Test.Presentation.Extensions;
using Test.Presentation.Middlewares;

namespace Test.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(w => w.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning)));


            // DI
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.BindConfigurations(builder.Configuration);

            // Controllers and Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

                options.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();
            

            // Middlewares
            app.UseMiddleware<ExceptionHandlingMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();
            app.MapControllers();

            
            // Seed the database with initial data
            await app.SeedDatabaseAsync();


            await app.RunAsync();
        }
    }
}
