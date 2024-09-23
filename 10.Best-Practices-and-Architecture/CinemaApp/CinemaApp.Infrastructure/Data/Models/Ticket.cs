using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Infrastructure.Data.Models;
public class Ticket
{
    [Key]
    public int Id { get; set; }

    //[StringLength(100)]
    //public string? CustomerName { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [Required]
    [Column(TypeName = "money")]
    public decimal BasePrice { get; set; }

    [Required]
    public int TariffId { get; set; }

    [ForeignKey(nameof(TariffId))]
    public Tariff Tariff { get; set; } = null!;

    [Required]
    public int SeatId { get; set; }
    [ForeignKey(nameof(SeatId))]
    public Seat Seat { get; set; } = null!;

    [Required]
    public int ScheduleId { get; set; }

    [ForeignKey(nameof(ScheduleId))]
    public Schedule Schedule { get; set; } = null!;
}
