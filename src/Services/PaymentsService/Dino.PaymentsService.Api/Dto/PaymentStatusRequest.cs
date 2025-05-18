using System.ComponentModel.DataAnnotations;

namespace Dino.PaymentsService.Api.Dto
{
    public class PaymentStatusRequest
    {
        [Required]
        public string Status { get; set; }
    }
}
