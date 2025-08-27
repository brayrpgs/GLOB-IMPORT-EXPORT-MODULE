using api.Enums;

namespace api.Models
{
    public class IssueType
    {
        public int Id { get; set; }
        public IssueTypeStatus Status { get; set; } 
        public IssueTypePriority Priority { get; set; } 
    }
}