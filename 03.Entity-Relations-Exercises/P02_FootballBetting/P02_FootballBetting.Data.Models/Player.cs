using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;
public class Player
{
    public Player()
    {
        PlayersStatistics = new HashSet<PlayerStatistic>();
    }
    [Key]
    public int PlayerId { get; set; }

    [MaxLength(ValidationConstants.PlayerNameMaxLength)]
    public string Name { get; set; } = null!;

    public int SquadNumber { get; set; }

    public bool IsInjured { get; set; }
    //*********************************************
    public int PositionId { get; set; }

    [ForeignKey(nameof(PositionId))]
    public virtual Position? Position { get; set; }
    //************************************************

    public int TeamId { get; set; }

    [ForeignKey(nameof(TeamId))]
    public virtual Team? Team { get; set; }
    //***********************************************
    public int? TownId { get; set; }

    [ForeignKey(nameof(TownId))]
    public Town? Town { get; set; }
    //*********************************

    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
}
