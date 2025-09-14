using api.Models;

namespace api.Mappings
{
    public static class IssueMapper
    {
        public static Issue? ToIssue(List<IssueFromCSV>? issueFromCSV)
        {
            if (issueFromCSV == null) return null;

            if (issueFromCSV[0] == null) return null;

            Issue issue = new();

            return null;

        }

    }
}