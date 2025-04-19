

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FullStackCrud.Server.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("age")]
        public int? Age { get; set; }
    }
}

