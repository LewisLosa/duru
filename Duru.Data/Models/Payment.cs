using System.ComponentModel.DataAnnotations.Schema;
using Duru.Data.Enums;

namespace Duru.Data.Models;

public class Payment : BaseEntity
{
    public int? ReservationId { get; set; }
    [ForeignKey(nameof(ReservationId))] public virtual Reservation? Reservation { get; set; }

    public int GuestId { get; set; }
    [ForeignKey(nameof(GuestId))] public virtual Guest Guest { get; set; } = null!;

    public decimal AmountPaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentStatus Status { get; set; }
    public string? Notes { get; set; }
}