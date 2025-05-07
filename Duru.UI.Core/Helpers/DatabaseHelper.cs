using Duru.UI.Core.Models;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Duru.UI.Core.Helpers;

public class DatabaseHelper
{
    private const string DatabaseFileName = "Duru.db";

    public SqliteConnection GetDbConnection() => new($"Data Source={DatabaseFileName}");

    public void EnsureDatabaseCreated()
    {
        if (!File.Exists(DatabaseFileName))
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            using var db = new SqliteConnection($"Data Source={DatabaseFileName}");
            db.Open();

            using var command = db.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Rooms (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Capacity INTEGER NOT NULL,
                    Type INTEGER NOT NULL
                );";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var name = reader.GetString(0);

                    Console.WriteLine($"Hello, {name}!");
                }
            }
        }
    }
}