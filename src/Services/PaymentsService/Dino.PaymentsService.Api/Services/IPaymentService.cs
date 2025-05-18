using Dino.PaymentsService.Api.DAL.Entities;
using Dino.PaymentsService.Api.Dto;

namespace Dino.PaymentsService.Api.Services
{
    public interface IPaymentService
    {
        Task<Payment> GetByIdAsync(Guid id);
        Task<IEnumerable<Payment>> GetByReservationIdAsync(Guid reservationId);
        Task<Payment> ProcessPaymentAsync(ProcessPaymentRequest request);
        Task<Payment> UpdatePaymentStatusAsync(Guid id, string status);
    }
}
