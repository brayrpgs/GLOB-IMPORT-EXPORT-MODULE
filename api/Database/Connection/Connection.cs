using Npgsql;

namespace api.Database.Connection
{
    public static class Connection
    {
        // Holds a single instance of the NpgsqlConnection (Singleton pattern)
        private static NpgsqlConnection? npgsqlConnection = null;

        // Creates and opens a new PostgreSQL connection using environment variables
        private static NpgsqlConnection GetConnection()
        {
            // Load connection settings from environment variables
            string? host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            string? database = Environment.GetEnvironmentVariable("POSTGRES_DB");
            string? username = Environment.GetEnvironmentVariable("POSTGRES_USER");
            string? password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

            // If any variable is missing → throw 500 error
            if (String.IsNullOrEmpty(host) || String.IsNullOrEmpty(database) || 
                String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                throw new Exception("500");

            // Build connection string for PostgreSQL
            string connString = $"Host={host};Database={database};Username={username};Password={password}";

            try
            {
                // Initialize and open connection
                NpgsqlConnection conn = new(connString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                // Log error and throw generic 500 error
                Console.WriteLine(ex.Message);
                throw new Exception("500");
            }
        }

        // Returns a single shared instance of the connection (lazy initialization)
        public static NpgsqlConnection GetInstance()
        {
            // If npgsqlConnection is null, create it. Otherwise, reuse it.
            npgsqlConnection ??= GetConnection();
            return npgsqlConnection;
        }

        // Properly close and dispose the connection
        public static void CloseConnection()
        {
            npgsqlConnection?.Close();
            npgsqlConnection?.Dispose();
            npgsqlConnection = null;
        }
    }
}
