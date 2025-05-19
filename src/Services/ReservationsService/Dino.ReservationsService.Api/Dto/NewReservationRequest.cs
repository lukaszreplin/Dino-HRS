namespace Dino.ReservationsService.Api.Dto
{
    public class NewReservationRequest
    {
        public DateOnly CheckIn { get; set; }

        public DateOnly CheckOut { get; set; }

        public Guid RoomId { get; set; }

        public Guid GuestId { get; set; }

        public double PricePerDay { get; set; }
    }
}
