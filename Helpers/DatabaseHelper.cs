using Grocero.Models;
using Microsoft.Data.Sqlite;

namespace Grocero.Helpers
{
    public class DatabaseHelper
    {
        // Path to the SQLite database
        private readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db");

        public DatabaseHelper()
        {
            // Establish a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Command to create the Users table if it does not exist
            var command = connection.CreateCommand();
            command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL
            );
            """;
            command.ExecuteNonQuery(); // Execute the command
        }

        public void AddUser(User user)
        {
            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the INSERT command
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
            // Bind parameters to avoid SQL injection
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.ExecuteNonQuery(); // Execute the command
        }

        public User GetUser(string email, string password)
        {
            // Open a connection to the database
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();

            // Prepare the SELECT command
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Email, Password FROM Users WHERE Email = @Email AND Password = @Password";
            // Bind parameters
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            // Execute the command and process the results
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3)
                };
            }

            return null; // Return null if the user is not found
        }
    }
}
