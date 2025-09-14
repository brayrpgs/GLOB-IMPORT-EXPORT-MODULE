using api.Models;
using api.Enums;
using api.Database.Connection;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class IssueRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public Issue? Post(Issue issue)
        {
            var conn = Connection.GetInstance();

            try
            {
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_ISSUE\"(" +
                    "@P_SUMMARY, @P_DESCRIPTION, @P_RESOLVE_AT, @P_DUE_DATE, @P_VOTES, @P_ORIGINAL_ESTIMATION, " +
                    "@P_CUSTOM_START_DATE, @P_STORY_POINT_ESTIMATE, @P_PARENT_SUMMARY, @P_ISSUE_TYPE, @P_PROJECT_ID, " +
                    "@P_USER_ASSIGNED, @P_USER_CREATOR, @P_USER_INFORMATOR, @P_SPRINT_ID, @P_STATUS, NULL)",
                    conn
                );

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

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                var json = reader.GetValue(0)?.ToString() ?? "{}";

                // Reemplazar valores especiales PostgreSQL
                json = json.Replace("\"-infinity\"", $"\"{DateTime.MinValue:yyyy-MM-dd}\"")
                           .Replace("\"infinity\"", $"\"{DateTime.MaxValue:yyyy-MM-dd}\"");

                var createdIssue = JsonSerializer.Deserialize<Issue>(json, JsonOptions);

                return createdIssue;
            }
            finally
            {
                Connection.CloseConnection();
            }
        }
    }
}
