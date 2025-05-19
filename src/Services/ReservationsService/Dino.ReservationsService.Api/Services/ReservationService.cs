using Dino.ReservationsService.Api.DAL;
using Dino.ReservationsService.Api.DAL.Entities;
using Dino.ReservationsService.Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace Dino.ReservationsService.Api.Services
{
    public class ReservationService(ReservationsDbContext reservationsDbContext) : IReservationService
    {
        public async Task<Guid> CreateReservation(NewReservationRequest request)
        {
            var reservation = new Reservation()
            {
                Id = Guid.NewGuid(),
                CheckIn = request.CheckIn,
                CheckOut = request.CheckOut,
                GuestId = request.GuestId,
                RoomId = request.RoomId,
                Status = "New"
            };
            await reservationsDbContext.Reservations.AddAsync(reservation);
            await reservationsDbContext.SaveChangesAsync();
            return reservation.Id;
        }

        public async Task<IEnumerable<ReservationListItemResponse>> GetAll()
        {
            return await reservationsDbContext.Reservations.Select(r => new ReservationListItemResponse()
            {
                Id = r.Id,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                RoomId = r.RoomId,
                GuestId = r.GuestId,
                Status = r.Status
            }).ToListAsync();
        }

        public async Task<ReservationResponse?> GetByIdAsync(Guid id)
        {
            return await reservationsDbContext
                .Reservations.Where(r => r.Id == id)
                .Select(r => new ReservationResponse()
                {
                    Id = r.Id,
                    CheckIn = r.CheckIn,
                    CheckOut = r.CheckOut,
                    RoomId = r.RoomId,
                    GuestId = r.GuestId,
                    Status = r.Status,
                    TotalPrice = 100.00,
                    PaymentStatus = "New"
                }).FirstOrDefaultAsync();
        }
    }
}
