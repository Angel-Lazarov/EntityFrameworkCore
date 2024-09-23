﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Infrastructure.Data.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int HallId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public int CinemaId { get; set; }

        [ForeignKey(nameof(CinemaId))]
        public Cinema Cinema { get; set; }

        [Required]
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(HallId))]
        public Hall Hall { get; set; } = null!;

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
