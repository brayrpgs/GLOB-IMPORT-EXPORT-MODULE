namespace api.Models
{
    public class IssueFromCSV
    {
        public string? Summary { get; set; }
        public string? Issueid { get; set; }
        public string? IssueType { get; set; }
        public string? Status { get; set; }
        public string? Projectname { get; set; }
        public string? Projectlead { get; set; }
        public string? Projectdescription { get; set; }
        public string? Priority { get; set; }
        public string? Resolution { get; set; }
        public string? Assignee { get; set; }
        public string? AssigneeId { get; set; }
        public string? Reporter { get; set; }
        public string? ReporterId { get; set; }
        public string? Creator { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Resolved { get; set; }
        public DateTime? Duedate { get; set; }
        public int? Votes { get; set; }
        public string? Description { get; set; }
        public string? Sprint { get; set; }
        public DateTime? CustomfieldStartdate { get; set; }
        public string? CustomfieldStorypointestimate { get; set; }
        public string? Parentkey { get; set; }
        public string? Parentsummary { get; set; }
    }
}