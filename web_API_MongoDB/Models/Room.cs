using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace web_API_MongoDB.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int RoomNumber { get; set; }
        public int DormitoryNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}