using System.Text.Json.Serialization;
using api.Enums;

namespace api.Models
{
    public class Issue
    {
        [JsonPropertyName("ISSUE_ID")]
        public int? Id { get; set; }
        public int? AuditId { get; set; }
        public int? Votes { get; set; }
        public int? OriginalEstimation { get; set; }
        public int? StoryPoints { get; set; }
        public int? IssueTypeId { get; set; }
        public long? ProjectId { get; set; }
        public int? ParentSummaryId { get; set; }
        public int? UserAssignedId { get; set; }
        public int? IssueUserCreatorId { get; set; }
        public int? IssueUserInformatorId { get; set; }
        public int? SprintId { get; set; }
        public string? Summary { get; set; } = "No summary";
        public string? Description { get; set; } = "No description";
        public DateTime? ResolveAt { get; set; } = DateTime.MaxValue;
        public DateTime? DueDate { get; set; } = DateTime.MaxValue;
        public DateTime? CustomStartDate { get; set; } = DateTime.MinValue;
        public IssueStatus? Status { get; set; }
    }
}