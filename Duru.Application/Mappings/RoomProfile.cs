using AutoMapper;
using Duru.Application.DTOs.Room;
using Duru.Domain.Models;

namespace Duru.Application.Mappings;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomDto>()
            .ReverseMap();
    }
}