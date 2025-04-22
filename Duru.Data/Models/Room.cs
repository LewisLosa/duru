using System.ComponentModel.DataAnnotations;
using Duru.Data.Enums;

namespace Duru.Data.Models;

public class Room : BaseEntity
{
    [Required] [MaxLength(10)] public string RoomNumber { get; set; } = string.Empty;

    [Required] [MaxLength(10)] public string Floor { get; set; } = string.Empty;

    public RoomType RoomType { get; set; }
    public RoomStatus Status { get; set; }
    public decimal PricePerNight { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}