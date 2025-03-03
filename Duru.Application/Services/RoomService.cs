using AutoMapper;
using Duru.Application.DTOs.Room;
using Duru.Application.Interfaces;
using Duru.Data.Repositories.Contracts;
using Duru.Domain.Enums;
using Duru.Domain.Models;

namespace Duru.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<RoomDto> CreateRoomAsync(RoomDto roomDto)
        {
            // RoomDto -> Room entity
            var newRoom = _mapper.Map<Room>(roomDto);
            
            newRoom.CreatedAt = DateTime.UtcNow;
            newRoom.UpdatedAt = DateTime.UtcNow;

            await _roomRepository.AddAsync(newRoom);
            return _mapper.Map<RoomDto>(newRoom);
        }

        public async Task UpdateRoomAsync(RoomDto roomDto)
        {
            var existingRoom = await _roomRepository.GetByIdAsync(roomDto.RoomId);
            if (existingRoom == null)
                throw new InvalidOperationException("Room not found"); 
            _mapper.Map(roomDto, existingRoom);
            existingRoom.UpdatedAt = DateTime.UtcNow;

            _roomRepository.Update(existingRoom);
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            var existingRoom = await _roomRepository.GetByIdAsync(roomId);
            if (existingRoom == null)
                throw new InvalidOperationException("Room not found");

            _roomRepository.Remove(existingRoom);
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByTypeAsync(RoomType roomType)
        {
            var rooms = await _roomRepository.GetRoomsByTypeAsync(roomType);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByTypeAndPriceAsync(RoomType roomType, int price)
        {
            var rooms = await _roomRepository.GetRoomsByTypeAndPriceAsync(roomType, price);
            return _mapper.Map<IEnumerable<RoomDto>>(rooms);
        }
    }
}