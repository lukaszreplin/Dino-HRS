using Microsoft.EntityFrameworkCore;

namespace Dino.ReservationsService.Api.DAL
{
    public class ReservationsDbContext(DbContextOptions<ReservationsDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Reservation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Entities.Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Entities.Reservation>()
                .HasIndex(r => new { r.RoomId, r.CheckIn, r.CheckOut });

            modelBuilder.Entity<Entities.Reservation>().HasData(
                new Entities.Reservation
                {
                    Id = Guid.Parse("aa5d6369-b31a-45d9-b39d-e1e2288818fa"),
                    RoomId = Guid.Parse("5df24c2f-3738-4920-a403-815dfa4a85dd"),
                    GuestId = Guid.Parse("fd0fb19a-8242-4c87-bb93-386e1a61552e"),
                    CheckIn = new DateOnly(2025, 5, 18),
                    CheckOut = new DateOnly(2025, 5, 20),
                    Status = "Confirmed"
                },
                new Entities.Reservation
                {
                    Id = Guid.Parse("c2592960-d746-4309-954f-2c88c1d0dc21"),
                    RoomId = Guid.Parse("a2905bdd-34bb-45e2-b1cd-1180159fde08"),
                    GuestId = Guid.Parse("f11bb0d5-31a3-4cc5-82f8-a2f224295d42"),
                    CheckIn = new DateOnly(2025, 6, 1),
                    CheckOut = new DateOnly(2025, 6, 8),
                    Status = "Confirmed"
                }
            );
        }
    }
}
