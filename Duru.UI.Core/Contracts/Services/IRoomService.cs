using Duru.UI.Core.Models;

namespace Duru.UI.Core.Contracts.Services;

public interface IRoomService
{
    Task<Room> GetRoomByIdAsync(int id);
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<bool> CreateRoomAsync(Room room);
    Task<bool> UpdateRoomAsync(Room room);
    Task<bool> DeleteRoomAsync(int id);
    Task<bool> IsRoomAvailableAsync(int roomId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Room>> GetGridDataAsync();
}