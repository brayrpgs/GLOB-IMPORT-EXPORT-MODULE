using api.Database.Connection;
using api.Models;
using Npgsql;
using System.Text.Json;

namespace api.Repository
{
    public class SprintRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public Sprint? Post(Sprint sprint)
        {
            // Get database connection instance
            var conn = Connection.GetInstance();

            try
            {
                using var cmd = new NpgsqlCommand(
                    "CALL PUBLIC.\"POST_SPRINT\"(@P_NAME, @P_DESCRIPTION, @P_DATE_INIT, @P_DATE_END, NULL)",
                    conn
                );

                // Assign input parameters with safe defaults
                cmd.Parameters.AddWithValue("P_NAME", NpgsqlTypes.NpgsqlDbType.Text, sprint.Name ?? "No name");
                cmd.Parameters.AddWithValue("P_DESCRIPTION", NpgsqlTypes.NpgsqlDbType.Text, sprint.Description ?? "No description");
                cmd.Parameters.AddWithValue("P_DATE_INIT", NpgsqlTypes.NpgsqlDbType.Date, sprint.DateInit ?? DateTime.MinValue);
                cmd.Parameters.AddWithValue("P_DATE_END", NpgsqlTypes.NpgsqlDbType.Date, sprint.DateEnd ?? DateTime.MaxValue);

                using var reader = cmd.ExecuteReader();

                if (!reader.Read()) return null;

                // Retrieve JSON result from DB
                var json = reader.GetValue(0)?.ToString() ?? "{}";

                // Normalize PostgreSQL special values to valid .NET values
                json = json.Replace("\"-infinity\"", $"\"{DateTime.MinValue:yyyy-MM-dd}\"")
                           .Replace("\"infinity\"", $"\"{DateTime.MaxValue:yyyy-MM-dd}\"");

                // Deserialize using cached JsonOptions
                var sprints = JsonSerializer.Deserialize<List<Sprint>>(json, JsonOptions);

                return sprints?.FirstOrDefault();
            }
            finally
            {
                // Always close connection
                Connection.CloseConnection();
            }
        }
    }
}
