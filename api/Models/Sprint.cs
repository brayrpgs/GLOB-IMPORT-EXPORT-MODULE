using System.Text.Json.Serialization;

namespace api.Models
{
    public class Sprint
    {
        [JsonPropertyName("SPRINT_ID")]
        public int? Id { get; set; }
        public string? Name { get; set; } = "No name";
        public string? Description { get; set; } = "No description";
        public DateTime? DateInit { get; set; } = DateTime.MinValue;
        public DateTime? DateEnd { get; set; } = DateTime.MaxValue;

    }
}