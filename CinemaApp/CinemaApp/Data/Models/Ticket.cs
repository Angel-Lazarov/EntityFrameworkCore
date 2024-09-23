using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? CustomerName { get; set; }

        [Required]
        public int TarrifId { get; set; }

        [ForeignKey(nameof(TarrifId))]
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
}
