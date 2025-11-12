using api.Enums;       
using api.Models;      
using api.Repository;  

namespace api.Services
{
    // Service class responsible for building and importing an Issue
    public class ImportService(Project? project, Issue? issue)
    {
        // Fields to store the provided project and issue instances
        private readonly Project? Project = project;
        private readonly Issue? issue = issue;

        // Builder-like methods to set various properties of the issue
        private ImportService SetSummary(string summary)
        {
            this.issue!.Summary = summary;
            return this;                   
        }

        private ImportService SetDescription(string description)
        {
            this.issue!.Description = description; 
            return this;
        }

        private ImportService SetResolveDate(DateTime resolveDate)
        {
            this.issue!.ResolveAt = resolveDate; 
            return this;
        }

        private ImportService SetDueDate(DateTime dueDate)
        {
            this.issue!.DueDate = dueDate;
            return this;
        }

        private ImportService SetVotes(int votes)
        {
            this.issue!.Votes = votes;
            return this;
        }

        private ImportService SetOriginalEstimation(int originalEstimation)
        {
            this.issue!.OriginalEstimation = originalEstimation;
            return this;
        }

        private ImportService SetCustomStartDate(DateTime customStartDate)
        {
            this.issue!.CustomStartDate = customStartDate; 
            return this;
        }

        private ImportService SetStoryPointEstimate(int storyPointEstimate)
        {
            this.issue!.StoryPoints = storyPointEstimate;
            return this;
        }

        private ImportService SetParentSummary(Issue issue)
        {
            this.issue!.ParentSummaryId = issue.Id;
            return this;
        }

        private ImportService SetIssueType(IssueType issueType)
        {
            this.issue!.IssueTypeId = issueType.Issue_Type_Id;
            return this;
        }

        private ImportService SetProjectId()
        {
            this.issue!.ProjectId = this.Project!.Id; 
            return this;
        }

        private ImportService SetSprintId(Sprint sprint)
        {
            this.issue!.SprintId = sprint.Id;
            return this;
        }

        private ImportService SetStatusIssue(IssueStatus issueStatus)
        {
            this.issue!.Status = issueStatus; 
            return this;
        }

        // Main method to construct an Issue from CSV data
        public Issue BuildIssue(IssueFromCSV ifcsv)
        {
            // Create IssueType instance from CSV, with fallback defaults
            IssueType issueType = new()
            {
                Status = Enum.TryParse<IssueTypeStatus>(ifcsv.IssueType, true, out var temp) ? temp : IssueTypeStatus.Other, // If parsing fails, default to Other
                Priority = (IssueTypePriority)Enum.Parse(typeof(IssueTypePriority), ifcsv.Priority ?? "Low") // Default priority is Low
            };

            // Insert IssueType into DB and retrieve the persisted record
            IssueType? issueTypeFromDB = new IssueTypeRepository().Post(issueType);

            // Create Sprint instance from CSV, defaulting to "In Backlog" if empty
            Sprint sprint = new()
            {
                Name = string.IsNullOrWhiteSpace(ifcsv.Sprint) ? "In Backlog" : ifcsv.Sprint
            };

            // Insert Sprint into DB and retrieve persisted record
            Sprint? sprintFromDB = new SprintRepository().Post(sprint);

            // Parse issue status from CSV, defaulting to "ToDo"
            IssueStatus issueStatus = (IssueStatus)Enum.Parse(typeof(IssueStatus), ifcsv.Status!.Replace(" ", "") ?? "ToDo");

            // Set all issue properties using the builder methods
            this.SetSummary(string.IsNullOrWhiteSpace(ifcsv.Summary) ? "No summary" : ifcsv.Summary)
                .SetDescription(string.IsNullOrWhiteSpace(ifcsv.Description) ? "No description" : ifcsv.Description)
                .SetResolveDate(ifcsv.Resolved ?? DateTime.MaxValue)
                .SetDueDate(ifcsv.Duedate ?? DateTime.MaxValue)
                .SetVotes(ifcsv.Votes ?? 0)
                .SetOriginalEstimation(0)
                .SetCustomStartDate(ifcsv.CustomfieldStartdate ?? DateTime.MinValue)
                .SetStoryPointEstimate(string.IsNullOrWhiteSpace(ifcsv.CustomfieldStorypointestimate) ? 0 : (int)double.Parse(ifcsv.CustomfieldStorypointestimate, System.Globalization.CultureInfo.InvariantCulture))
                //.SetParentSummary(ifcsv.Parentsummary)
                .SetIssueType(issueTypeFromDB!)
                .SetProjectId()
                .SetSprintId(sprintFromDB ?? new Sprint { Id = null });

            // Insert the issue into DB and return the persisted record
            Issue? issueInserted = new IssueRepository().Post(issue!);

            return issueInserted!;
        }
    }
}
