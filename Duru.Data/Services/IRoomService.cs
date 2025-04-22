using Duru.Data.Models;

namespace Duru.Data.Services;

public interface IRoomService
{
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomByIdAsync(int id);
    Task<Room?> GetRoomByNumberAsync(string roomNumber);
    Task AddRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(int id);
    Task<bool> RoomExistsAsync(int id);
}