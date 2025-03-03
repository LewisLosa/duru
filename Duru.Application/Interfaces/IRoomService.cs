using Duru.Application.DTOs.Room;
using Duru.Domain.Enums;

namespace Duru.Application.Interfaces;

public interface IRoomService
{
    Task<RoomDto> GetRoomByIdAsync(int roomId);
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto> CreateRoomAsync(RoomDto roomDto);
    Task UpdateRoomAsync(RoomDto roomDto);
    Task DeleteRoomAsync(int roomId);
    
    Task<IEnumerable<RoomDto>> GetRoomsByTypeAsync(RoomType roomType);
    Task<IEnumerable<RoomDto>> GetRoomsByTypeAndPriceAsync(RoomType roomType, int price);

}