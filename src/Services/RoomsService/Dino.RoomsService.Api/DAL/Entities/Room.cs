using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dino.RoomsService.Api.DAL.Entities
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("capacity")]
        public int Capacity { get; set; }

        [BsonElement("hasBathroom")]
        public bool HasBathroom { get; set; }

        [BsonElement("hasBalcony")]
        public bool HasBalcony { get; set; }

        [BsonElement("bedType")]
        public string BedType { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }
    }
}
