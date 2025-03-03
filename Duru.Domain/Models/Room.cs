using Duru.Domain.Enums;
namespace Duru.Domain.Models;

public class Room
{
    public int RoomId { get; set; }
    public string? RoomName { get; set; }
    public int RoomCapacity { get; set; }
    public int RoomPrice { get; set; }
    public RoomType RoomType { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}