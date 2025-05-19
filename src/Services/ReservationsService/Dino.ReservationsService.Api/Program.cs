
using Dino.ReservationsService.Api.DAL;
using Dino.ReservationsService.Api.Dto;
using Dino.ReservationsService.Api.Services;
using Microsoft.AspNetCore.Mvc;
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

            builder.Services.AddScoped<IReservationService, ReservationService>();

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

            var paymentsApi = app.MapGroup("/reservations");

            paymentsApi.MapGet("/", async (IReservationService reservationService) =>
                Results.Ok(await reservationService.GetAll()))
                .WithName("Get all")
                .WithOpenApi();

            paymentsApi.MapGet("/{id}", async (IReservationService reservationService, Guid id) =>
            {
                var reservation = await reservationService.GetByIdAsync(id);
                return reservation != null ? Results.Ok(reservation) : Results.NotFound();
            })
            .WithName("Get reservation by Id")
            .WithOpenApi();

            paymentsApi.MapPost("/", async ([FromBody] NewReservationRequest request, IReservationService reservationService) =>
            {
                var response = await reservationService.CreateReservation(request);

                return Results.Created($"/api/reservations/{response}", response);
            })
            .WithName("Get payment by reservation Id")
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
