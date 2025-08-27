using api.Enums;

namespace api.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int AuditId { get; set; }
        public int Votes { get; set; }
        public int OriginalEstimation { get; set; }
        public int StoryPoints { get; set; }
        public int IssueTypeId { get; set; }
        public int ProjectId { get; set; }
        public int ParentSummaryId { get; set; }
        public int UserAssignedId { get; set; }
        public int IssueUserCreatorId { get; set; }
        public int IssueUserInformatorId { get; set; }
        public int SprintId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ResolveAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CustomStartDate { get; set; }

        public IssueStatus Status { get; set; }
    }
}