using api.Models;

namespace api.Mappings
{
    public static class ProjectMapper
    {
        public static Project? ToProject(List<IssueFromCSV>? issueFromCSV)
        {
            // Validate that the list is not null and has at least one item
            if (issueFromCSV == null || issueFromCSV.Count == 0) return null;

            var first = issueFromCSV[0];

            // Extra check in case the first record is null
            if (first == null) return null;

            // Map relevant fields to a Project entity
            Project project = new()
            {
                Name = first.Projectname,
                Description = string.IsNullOrWhiteSpace(first.Projectdescription) ? "No project description": first.Projectdescription
            };

            return project;
        }
    }
}
