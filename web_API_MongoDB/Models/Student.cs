using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace web_API_MongoDB.Models
{
    /// <summary>
    /// Представляє сутність Студента в базі даних MongoDB.
    /// Також використовується як модель Користувача для аутентифікації.
    /// </summary>
    public class Student
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; } 

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [BsonElement("email")] 
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public int Course { get; set; }

        [BsonElement("passwordHash")] 
        public string PasswordHash { get; set; } = string.Empty;
    }
}