using api.Database.Connection;
using api.Enums;
using api.Models;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class IssueTypeRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public IssueType? Post(IssueType issueType)
        {
            var conn = Connection.GetInstance();

            try
            {
                // Prepare command for the stored procedure
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_ISSUE_TYPE\"(@P_STATUS, @P_PRIORITY, NULL)", conn
                );

                // Assign parameters with safe fallbacks (avoid null DB values)
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

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                // Retrieve JSON result from the first column
                var json = reader.GetValue(0)?.ToString() ?? "[]";

                // Deserialize JSON into a list of IssueType objects using cached options
                var issueTypes = JsonSerializer.Deserialize<List<IssueType>>(json, JsonOptions);

                // Return the first IssueType from the list (if any)
                return issueTypes?.FirstOrDefault();
            }
            finally
            {
                // Ensure DB connection is always closed, even on exception
                Connection.CloseConnection();
            }
        }
    }
}
