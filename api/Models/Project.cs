using api.Enums;

namespace api.Models

{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserProjectId { get; set; }
        public DateTime DateInit { get; set; }
        public DateTime? DateEnd { get; set; }
        public ProjectStatus Status { get; set; } 
        public decimal Progress { get; set; }

    }
}