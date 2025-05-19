namespace Dino.ReservationsService.Api.Dto
{
    public class ReservationListItemResponse
    {
        public Guid Id { get; set; }

        public DateOnly CheckIn { get; set; }

        public DateOnly CheckOut { get; set; }

        public Guid RoomId { get; set; }

        public Guid GuestId { get; set; }

        public string Status { get; set; }
    }
}
