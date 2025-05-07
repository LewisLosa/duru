using Duru.UI.Core.Contracts.Services;
using Duru.UI.Core.Helpers;
using Duru.UI.Core.Models;
using Microsoft.Data.Sqlite;

namespace Duru.UI.Core.Services;

public class RoomService : IRoomService
{
    private readonly DatabaseHelper _dbHelper;

    public RoomService(DatabaseHelper dbHelper)
    {
        _dbHelper = dbHelper;
        _dbHelper.EnsureDatabaseCreated();
    }

    public async Task<Room> GetRoomByIdAsync(int id)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("SELECT * FROM Rooms WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Room
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Capacity = reader.GetInt32(2),
                Type = (RoomType)reader.GetInt32(3)
            };
        }

        return null;
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        var rooms = new List<Room>();
        using (var connection = _dbHelper.GetDbConnection())
        {
            await connection.OpenAsync();
            var command = new SqliteCommand("SELECT * FROM Rooms", connection);
            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                rooms.Add(new Room
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Capacity = reader.GetInt32(2),
                    Type = (RoomType)reader.GetInt32(3)
                });
            }
        }

        return rooms;
    }

    public async Task<bool> CreateRoomAsync(Room room)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("INSERT INTO Rooms (Name, Capacity, Type) VALUES (@Name, @Capacity, @Type)",
            connection);
        command.Parameters.AddWithValue("@Name", room.Name);
        command.Parameters.AddWithValue("@Capacity", room.Capacity);
        command.Parameters.AddWithValue("@Type", (int)room.Type);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> UpdateRoomAsync(Room room)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command =
            new SqliteCommand("UPDATE Rooms SET Name = @Name, Capacity = @Capacity, Type = @Type WHERE Id = @Id",
                connection);
        command.Parameters.AddWithValue("@Id", room.Id);
        command.Parameters.AddWithValue("@Name", room.Name);
        command.Parameters.AddWithValue("@Capacity", room.Capacity);
        command.Parameters.AddWithValue("@Type", (int)room.Type);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command = new SqliteCommand("DELETE FROM Rooms WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate)
    {
        await using var connection = _dbHelper.GetDbConnection();
        await connection.OpenAsync();
        var command =
            new SqliteCommand(
                "SELECT COUNT(*) FROM Reservations WHERE RoomId = @RoomId AND ((StartDate <= @EndDate AND EndDate >= @StartDate))",
                connection);
        command.Parameters.AddWithValue("@RoomId", roomId);
        command.Parameters.AddWithValue("@StartDate", startDate);
        command.Parameters.AddWithValue("@EndDate", endDate);
        var count = (long)await command.ExecuteScalarAsync();
        return count == 0;
    }
    private List<Room> _allRooms;

    public async Task<IEnumerable<Room>> GetGridDataAsync()
    {
        _allRooms ??= new List<Room>(await GetAllRoomsAsync());

        await Task.CompletedTask;
        return _allRooms;
    }
}