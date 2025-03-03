using Duru.Data.Repositories.Base;
using Duru.Domain.Enums;
using Duru.Domain.Models;

namespace Duru.Data.Repositories.Contracts;

public interface IRoomRepository : IRepository<Room>
{
    Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomType roomType);
    Task<IEnumerable<Room>> GetRoomsByTypeAndPriceAsync(RoomType roomType, int roomPrice);
}