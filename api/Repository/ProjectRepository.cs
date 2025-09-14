using api.Database.Connection;
using api.Enums;
using api.Models;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class ProjectRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public List<Project>? PostProject(Project project)
        {
            var conn = Connection.GetInstance();

            try
            {
                // Prepare command for the stored procedure
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_PROJECT\"(@P_NAME, @P_DESCRIPTION, @P_USER_PROJECT_ID_FK, @P_DATE_INIT, @P_DATE_END, @P_STATUS, @P_PROGRESS, NULL)",
                    conn
                );

                // Assign parameters with safe fallbacks (avoiding null DB values)
                cmd.Parameters.AddWithValue("P_NAME", NpgsqlTypes.NpgsqlDbType.Text, project.Name ?? "No name");
                cmd.Parameters.AddWithValue("P_DESCRIPTION", NpgsqlTypes.NpgsqlDbType.Text, project.Description ?? "No description");
                cmd.Parameters.AddWithValue("P_USER_PROJECT_ID_FK", NpgsqlTypes.NpgsqlDbType.Bigint, project.UserProjectId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("P_DATE_INIT", NpgsqlTypes.NpgsqlDbType.Date, project.DateInit ?? DateTime.MinValue);
                cmd.Parameters.AddWithValue("P_DATE_END", NpgsqlTypes.NpgsqlDbType.Date, project.DateEnd ?? DateTime.MaxValue);
                cmd.Parameters.AddWithValue("P_STATUS", NpgsqlTypes.NpgsqlDbType.Bigint, (long)(project.Status ?? ProjectStatus.NotStarted));
                cmd.Parameters.AddWithValue("P_PROGRESS", NpgsqlTypes.NpgsqlDbType.Numeric, project.Progress ?? 0);

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                // Retrieve JSON result
                var json = reader.GetValue(0)?.ToString() ?? "[]";

                // Replace PostgreSQL special values (-infinity / infinity) with valid .NET equivalents
                json = json.Replace("\"-infinity\"", $"\"{DateOnly.MinValue:yyyy-MM-dd}\"")
                           .Replace("\"infinity\"", $"\"{DateOnly.MaxValue:yyyy-MM-dd}\"");

                // Deserialize JSON into list of Project objects using cached options
                var projects = JsonSerializer.Deserialize<List<Project>>(json, JsonOptions);

                return projects;
            }
            finally
            {
                // Ensure DB connection is always closed
                Connection.CloseConnection();
            }
        }
    }
}
