using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Team
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = new List<TeamFootballer>();

    }
}
