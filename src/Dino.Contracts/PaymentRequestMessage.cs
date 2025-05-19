namespace Dino.Contracts
{
    public record PaymentRequestMessage
    {
        public Guid ReservationId { get; init; }

        public decimal Amount { get; init; }
    }
}
