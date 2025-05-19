using Dino.Contracts;
using Dino.PaymentsService.Api.Dto;
using MassTransit;

namespace Dino.PaymentsService.Api.Services
{
    public class PaymentRequestConsumer : IConsumer<PaymentRequestMessage>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentRequestConsumer> _logger;

        public PaymentRequestConsumer(
            IPaymentService paymentService,
            ILogger<PaymentRequestConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentRequestMessage> context)
        {
            var message = context.Message;

            _logger.LogInformation("Received payment request for reservation: {ReservationId}",
                message.ReservationId);

            try
            {
                var paymentRequest = new ProcessPaymentRequest
                {
                    ReservationId = message.ReservationId,
                    Amount = message.Amount,
                    Currency = "PLN",
                    PaymentMethod = "Automatic"
                };

                var payment = await _paymentService.ProcessPaymentAsync(paymentRequest);

                _logger.LogInformation("Payment {PaymentId} processed for reservation {ReservationId} with status: {Status}",
                    payment.Id, message.ReservationId, payment.Status);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for reservation: {ReservationId}",
                    message.ReservationId);

                throw;
            }
        }
    }
}
