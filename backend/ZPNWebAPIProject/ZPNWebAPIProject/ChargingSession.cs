using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ZPNWebAPIProject
{
    public class ChargingSession 
    {
      
        [JsonIgnore]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId, AllowTruncation = true)]
        public ObjectId Id { get; set; }
        //

        [BsonRepresentation(MongoDB.Bson.BsonType.String, AllowTruncation = true)]
        public Guid SessionId { get; set; }

        //

        [BsonRepresentation(BsonType.DateTime, AllowTruncation = true)]
        public DateTime StartTime { get; set; }

        [BsonRepresentation(BsonType.DateTime, AllowTruncation = true)]
        public DateTime? EndTime { get; set; }

        [JsonIgnore]
        [BsonRepresentation(BsonType.String, AllowTruncation = true)]
        public string? Status { get; set; }

        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double EnergyConsumed { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (EndTime <= StartTime)
        //    {
        //        yield return new ValidationResult("End Time must be greater than the start Time.", new[] { "EndTime" });
        //    }
        //}

    }
}
