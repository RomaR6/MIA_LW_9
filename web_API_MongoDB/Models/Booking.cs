using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace web_API_MongoDB.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string StudentId { get; set; } = null!;

        [BsonRepresentation(BsonType.ObjectId)] 
        public string RoomId { get; set; } = null!;

        public DateTime BookingDate { get; set; }
    }
}