// Infrastruktur/DbContext/YourDataContext.cs
using System.Diagnostics;
using Domain.Entities;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Data
{
    public class AppDbContext : DataConnection
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString) : base("SQLite", connectionString)
        {
            _connectionString = connectionString;
            Init();
        }

        public ITable<Book> Books => this.GetTable<Book>();
        public ITable<User> Users => this.GetTable<User>();

        private void Init()
        {
            var dbFilePath = new SqliteConnectionStringBuilder(_connectionString).DataSource;

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            if (!TableExists(connection, "Book"))
            {
                this.CreateTable<Book>();
            }

            if (!TableExists(connection, "User"))
            {
                this.CreateTable<User>();
            }
        }
        private bool TableExists(SqliteConnection connection, string tableName)
        {
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
            var result = command.ExecuteScalar();
            return result != null;
        }
    }
}
