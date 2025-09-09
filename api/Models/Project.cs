using api.Enums;

namespace api.Models

{
    public class Project
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } = "No description";
        public int? UserProjectId { get; set; }
        public DateTime? DateInit { get; set; } = DateTime.Now;
        public DateTime? DateEnd { get; set; } = DateTime.Now;
        public ProjectStatus? Status { get; set; }
        public decimal? Progress { get; set; } = 0;

    }
}