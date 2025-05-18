using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dino.PaymentsService.Api.DAL.Entities
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("reservationId")]
        public Guid ReservationId { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("currency")]
        public string Currency { get; set; } = "PLN";

        [BsonElement("method")]
        public string PaymentMethod { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
