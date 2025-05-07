using Duru.UI.Core.Contracts.Services;
using Duru.UI.Core.Helpers;
using Duru.UI.Core.Models;
using Microsoft.Data.Sqlite;

namespace Duru.UI.Core.Services;

public class UserService : IUserService
{
    private readonly DatabaseHelper _dbHelper;

    public UserService(DatabaseHelper dbHelper)
    {
        _dbHelper = dbHelper;
        _dbHelper.EnsureDatabaseCreated();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("SELECT * FROM Users WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                Role = (Role)reader.GetInt32(4)
            };
        }

        return null;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("SELECT * FROM Users WHERE Email = @Email", connection);
        command.Parameters.AddWithValue("@Email", email);
        var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                Role = (Role)reader.GetInt32(4)
            };
        }

        return null;
    }

    public async Task<User> GetUserByPhoneAsync(string phoneNumber)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("SELECT * FROM Users WHERE PhoneNumber = @PhoneNumber", connection);
        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
        var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                Role = (Role)reader.GetInt32(4)
            };
        }

        return null;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = new List<User>();
        using (var connection = _dbHelper.GetDbConnection())
        {
            await connection.OpenAsync();
            var command = new SqliteCommand("SELECT * FROM Users", connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    Role = (Role)reader.GetInt32(4)
                });
            }
        }

        return users;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command =
            new SqliteCommand(
                "INSERT INTO Users (Name, Email, PhoneNumber, Role) VALUES (@Name, @Email, @PhoneNumber, @Role)",
                connection);
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        command.Parameters.AddWithValue("@Role", (int)user.Role);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command =
            new SqliteCommand(
                "UPDATE Users SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, Role = @Role WHERE Id = @Id",
                connection);
        command.Parameters.AddWithValue("@Id", user.Id);
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        command.Parameters.AddWithValue("@Role", (int)user.Role);
        return await command.ExecuteNonQueryAsync() > 0;
    }


    public async Task<bool> DeleteUserAsync(int id)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("DELETE FROM Users WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> AuthenticateUserAsync(string email, string hashedPassword)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command =
            new SqliteCommand("SELECT * FROM Users WHERE Email = @Email AND HashedPassword = @HashedPassword",
                connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
        var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
}