using api.Models;
using api.Database.Connection;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class IssueRepository
    {
        // JSON serializer options: case-insensitive property names
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public Issue? Post(Issue issue)
        {
            // Get the database connection instance
            var conn = Connection.GetInstance();

            try
            {
                // Create a new NpgsqlCommand to call the stored procedure "POST_ISSUE"
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_ISSUE\"(" +
                    "@P_SUMMARY, @P_DESCRIPTION, @P_RESOLVE_AT, @P_DUE_DATE, @P_VOTES, @P_ORIGINAL_ESTIMATION, " +
                    "@P_CUSTOM_START_DATE, @P_STORY_POINT_ESTIMATE, @P_PARENT_SUMMARY, @P_ISSUE_TYPE, @P_PROJECT_ID, " +
                    "@P_USER_ASSIGNED, @P_USER_CREATOR, @P_USER_INFORMATOR, @P_SPRINT_ID, @P_STATUS, NULL)",
                    conn
                );

                // Add parameters for the stored procedure, using defaults if null
                cmd.Parameters.AddWithValue("P_SUMMARY", NpgsqlTypes.NpgsqlDbType.Text, issue.Summary ?? "No summary");
                cmd.Parameters.AddWithValue("P_DESCRIPTION", NpgsqlTypes.NpgsqlDbType.Text, issue.Description ?? "No description");
                cmd.Parameters.AddWithValue("P_RESOLVE_AT", NpgsqlTypes.NpgsqlDbType.Date, issue.ResolveAt ?? DateTime.MaxValue);
                cmd.Parameters.AddWithValue("P_DUE_DATE", NpgsqlTypes.NpgsqlDbType.Date, issue.DueDate ?? DateTime.MaxValue);
                cmd.Parameters.AddWithValue("P_VOTES", NpgsqlTypes.NpgsqlDbType.Bigint, issue.Votes ?? 0);
                cmd.Parameters.AddWithValue("P_ORIGINAL_ESTIMATION", NpgsqlTypes.NpgsqlDbType.Bigint, issue.OriginalEstimation ?? 0);
                cmd.Parameters.AddWithValue("P_CUSTOM_START_DATE", NpgsqlTypes.NpgsqlDbType.Date, issue.CustomStartDate ?? DateTime.MinValue);
                cmd.Parameters.AddWithValue("P_STORY_POINT_ESTIMATE", NpgsqlTypes.NpgsqlDbType.Bigint, issue.StoryPoints ?? 0);
                cmd.Parameters.AddWithValue("P_PARENT_SUMMARY", NpgsqlTypes.NpgsqlDbType.Bigint, issue.ParentSummaryId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_ISSUE_TYPE", NpgsqlTypes.NpgsqlDbType.Bigint, issue.IssueTypeId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_PROJECT_ID", NpgsqlTypes.NpgsqlDbType.Bigint, issue.ProjectId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_USER_ASSIGNED", NpgsqlTypes.NpgsqlDbType.Bigint, issue.UserAssignedId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_USER_CREATOR", NpgsqlTypes.NpgsqlDbType.Bigint, issue.IssueUserCreatorId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_USER_INFORMATOR", NpgsqlTypes.NpgsqlDbType.Bigint, issue.IssueUserInformatorId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_SPRINT_ID", NpgsqlTypes.NpgsqlDbType.Bigint, issue.SprintId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_STATUS", NpgsqlTypes.NpgsqlDbType.Bigint, (long?)(issue.Status ?? 0));

                // Execute the command and get the result
                using var reader = cmd.ExecuteReader();

                // If no rows returned, insertion failed
                if (!reader.Read())
                    return null;

                // Get JSON string from the first column
                var json = reader.GetValue(0)?.ToString() ?? "{}";

                // Replace PostgreSQL special values for dates
                json = json.Replace("\"-infinity\"", $"\"{DateTime.MinValue:yyyy-MM-dd}\"")
                           .Replace("\"infinity\"", $"\"{DateTime.MaxValue:yyyy-MM-dd}\"");

                // Deserialize JSON into Issue object
                var createdIssue = JsonSerializer.Deserialize<Issue>(json, JsonOptions);

                // Return the created Issue
                return createdIssue;
            }
            finally
            {
                // Close the database connection
                Connection.CloseConnection();
            }
        }
    }
}
