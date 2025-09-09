using api.Models;

namespace api.Mappings
{
    public static class ProjectMapper
    {
        public static Project? ToProject(List<IssueFromCSV>? issueFromCSV)
        {
            if (issueFromCSV == null) return null;

            if (issueFromCSV[0] == null) return null;

            Project project = new();

            project.Name = issueFromCSV[0].Projectname;

            project.Description = issueFromCSV[0].Projectdescription;

            return project;

        }
    }
}