using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Position
{
    public Position()
    {
        Players = new HashSet<Player>();
    }
    public int PositionId { get; set; }

    [MaxLength(20)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Player> Players { get; set; }
}
