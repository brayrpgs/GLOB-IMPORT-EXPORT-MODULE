namespace api.Models
{
    public class IssueFromCSV
    {
        public string? Summary { get; set; } = "No summary";
        public string? Issueid { get; set; } 
        public string? IssueType { get; set; } 
        public string? Status { get; set; } 
        public string? Projectname { get; set; } = "No name";
        public string? Projectlead { get; set; }
        public string? Projectdescription { get; set; } = "No description";
        public string? Priority { get; set; }
        public string? Resolution { get; set; }
        public string? Assignee { get; set; }
        public string? AssigneeId { get; set; }
        public string? Reporter { get; set; }
        public string? ReporterId { get; set; }
        public string? Creator { get; set; }
        public string? CreatorId { get; set; }
        public DateTime? Created { get; set; } = DateTime.MinValue;
        public DateTime? Resolved { get; set; } = DateTime.MaxValue;
        public DateTime? Duedate { get; set; } = DateTime.MaxValue;
        public int? Votes { get; set; }
        public string? Description { get; set; } = "No description";
        public string? Sprint { get; set; }
        public DateTime? CustomfieldStartdate { get; set; } = DateTime.MinValue;
        public string? CustomfieldStorypointestimate { get; set; }
        public string? Parentkey { get; set; }
        public string? Parentsummary { get; set; }
    }
}