using System.ComponentModel.DataAnnotations.Schema;
using Duru.Data.Enums;

namespace Duru.Data.Models;

public class Reservation : BaseEntity
{
    public int GuestId { get; set; }
    [ForeignKey(nameof(GuestId))] public virtual Guest Guest { get; set; } = null!;

    public int RoomId { get; set; }
    [ForeignKey(nameof(RoomId))] public virtual Room Room { get; set; } = null!;

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public ReservationStatus Status { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}