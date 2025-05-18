
using Dino.ReservationsService.Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace Dino.ReservationsService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ReservationsDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));

            // Dodaj automatyczne migracje przy starcie
            builder.Services.AddHostedService<MigrationService>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/reservations", (HttpContext httpContext) =>
            {
                return "All reservations ok";
            })
            .WithName("Get reservations")
            .WithOpenApi();

            app.Run();
        }

        public static async Task MigrateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var maxRetryAttempts = 10;
            var retryDelay = TimeSpan.FromSeconds(5);

            for (int i = 0; i < maxRetryAttempts; i++)
            {
                try
                {
                    logger.LogInformation("Attempting to migrate database, attempt {Attempt}", i + 1);
                    var context = services.GetRequiredService<ReservationsDbContext>();
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migration completed successfully");
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database, retrying in {Delay} seconds", retryDelay.TotalSeconds);
                    await Task.Delay(retryDelay);
                }
            }

            logger.LogCritical("Could not migrate database after {Attempts} attempts", maxRetryAttempts);
        }
    }
}
