using Microsoft.Data.Sqlite;

namespace WaiterBackend.Services.Databases
{

    public class Database
    {
        private readonly string _connectionString = "Data Source=app.db";

        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}