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
        public ObjectId Id { get; set; }

        public DateTime StartTime { get; set; }
             
        public DateTime? EndTime { get; set; }

        [JsonIgnore]
        public string? Status { get; set; }

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
