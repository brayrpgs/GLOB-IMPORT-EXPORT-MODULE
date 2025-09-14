using System.Text.Json.Serialization;
using api.Enums;

namespace api.Models
{
    public class Project
    {
        [JsonPropertyName("PROJECT_ID")]
        public long? Id { get; set; }

        [JsonPropertyName("NAME")]
        public string? Name { get; set; }

        [JsonPropertyName("DESCRIPTION")]
        public string? Description { get; set; }

        [JsonPropertyName("USER_PROJECT_ID_FK")]
        public long? UserProjectId { get; set; }

        [JsonPropertyName("DATE_INIT")]
        public DateTime? DateInit { get; set; }

        [JsonPropertyName("DATE_END")]
        public DateTime? DateEnd { get; set; }

        [JsonPropertyName("STATUS")]
        public ProjectStatus? Status { get; set; }

        [JsonPropertyName("PROGRESS")]
        public decimal? Progress { get; set; }
    }
}
