using System.ComponentModel.DataAnnotations;

namespace Duru.Data.Models;

public class Guest : BaseEntity
{
    [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;

    [MaxLength(255)] [EmailAddress] public string? Email { get; set; }

    [MaxLength(20)] public string? PhoneNumber { get; set; }

    [MaxLength(100)] public string? Country { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}