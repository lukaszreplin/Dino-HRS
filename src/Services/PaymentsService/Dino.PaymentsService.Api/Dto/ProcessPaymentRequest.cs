using System.ComponentModel.DataAnnotations;

namespace Dino.PaymentsService.Api.Dto
{
    public class ProcessPaymentRequest
    {
        [Required]
        public Guid ReservationId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "PLN";

        [Required]
        public string PaymentMethod { get; set; }
    }
}
