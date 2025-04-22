using Duru.Data.Models;
using Duru.Data.Repositories;
namespace Duru.Data.Services;

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;
    private readonly IRepository<Room> _roomRepository;
    
    public RoomService(IRepository<Room> roomRepository, ApplicationDbContext context)
    {
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        return await _roomRepository.GetByIdAsync(id);
    }
    public async Task<Room?> GetRoomByNumberAsync(string roomNumber)
    {
        var rooms = await _roomRepository.FindAsync(r => r.RoomNumber == roomNumber);
        return rooms.FirstOrDefault();
    }


    public async Task AddRoomAsync(Room room)
    {
        await _roomRepository.AddAsync(room);
        await _context.SaveChangesAsync(); 
    }

    public async Task UpdateRoomAsync(Room room)
    {
        _roomRepository.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRoomAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room != null)
        {
            _roomRepository.Delete(room);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> RoomExistsAsync(int id)
    {
        return await _roomRepository.GetByIdAsync(id) != null;
    }
}