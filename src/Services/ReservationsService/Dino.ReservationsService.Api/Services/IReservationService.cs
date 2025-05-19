using Dino.ReservationsService.Api.Dto;

namespace Dino.ReservationsService.Api.Services
{
    public interface IReservationService
    {
        Task<ReservationResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservationListItemResponse>> GetAll();
        Task<Guid> CreateReservation(NewReservationRequest request);
    }
}
