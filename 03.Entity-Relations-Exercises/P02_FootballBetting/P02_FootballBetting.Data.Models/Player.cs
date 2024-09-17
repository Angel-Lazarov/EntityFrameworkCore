using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    public Player()
    {
        PlayersStatistics = new HashSet<PlayerStatistic>();
    }
    public int PlayerId { get; set; }

    [MaxLength(60)]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }

    public bool IsInjured { get; set; }
    //**************************************
    public int PositionId { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; } = null!;
    //************************************
    public int TeamId { get; set; }
    [ForeignKey(nameof(TeamId))]
    public virtual Team Team { get; set; } = null!;

    //#####################################################
    public int TownId { get; set; }
    [ForeignKey(nameof(TownId))]
    public virtual Town Town { get; set; } = null!;
    //#####################################################
    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
}
