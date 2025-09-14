using api.Database.Connection;
using api.Enums;
using api.Models;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class IssueTypeRepository
    {
        // JSON serializer options: case-insensitive property names
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public IssueType? Post(IssueType issueType)
        {
            // Get database connection
            var conn = Connection.GetInstance();

            try
            {
                // Prepare command for the stored procedure "POST_ISSUE_TYPE"
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_ISSUE_TYPE\"(@P_STATUS, @P_PRIORITY, NULL)", conn
                );

                // Assign parameters with safe fallbacks to avoid nulls
                cmd.Parameters.AddWithValue(
                    "P_STATUS",
                    NpgsqlTypes.NpgsqlDbType.Bigint,
                    (long)(issueType.Status ?? IssueTypeStatus.Task)
                );

                cmd.Parameters.AddWithValue(
                    "P_PRIORITY",
                    NpgsqlTypes.NpgsqlDbType.Bigint,
                    (long)(issueType.Priority ?? IssueTypePriority.Low)
                );

                // Execute the command and get the result
                using var reader = cmd.ExecuteReader();

                // If no rows returned, insertion failed
                if (!reader.Read())
                    return null;

                // Retrieve JSON result from the first column
                var json = reader.GetValue(0)?.ToString() ?? "[]";

                // Deserialize JSON into a list of IssueType objects
                var issueTypes = JsonSerializer.Deserialize<List<IssueType>>(json, JsonOptions);

                // Return the first IssueType from the list, if available
                return issueTypes?.FirstOrDefault();
            }
            finally
            {
                // Ensure DB connection is closed even if an exception occurs
                Connection.CloseConnection();
            }
        }
    }
}
