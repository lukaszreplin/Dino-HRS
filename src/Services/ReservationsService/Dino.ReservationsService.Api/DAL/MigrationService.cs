using Microsoft.EntityFrameworkCore;

namespace Dino.ReservationsService.Api.DAL
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(
            IServiceProvider serviceProvider,
            ILogger<MigrationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ReservationsDbContext>();

            try
            {
                _logger.LogInformation("Applying migrations...");
                await dbContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migrations applied successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
