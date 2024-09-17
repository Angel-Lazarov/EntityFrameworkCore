using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    public Game()
    {
        PlayersStatistics = new HashSet<PlayerStatistic>();
        Bets = new HashSet<Bet>();
    }
    public int GameId { get; set; }

    public int HomeTeamId { get; set; }
    [ForeignKey(nameof(HomeTeamId))]
    public virtual Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }
    [ForeignKey(nameof(AwayTeamId))]
    public virtual Team AwayTeam { get; set; } = null!;

    public byte HomeTeamGoals { get; set; }
    public byte AwayTeamGoals { get; set; }

    public double HomeTeamBetRate { get; set; }
    public double AwayTeamBetRate { get; set; }
    public double DrawBetRate { get; set; }

    [Required]
    public DateTime DateTime { get; set; }

    [MaxLength(10)]
    public string Result { get; set; } = null!;

    public virtual ICollection<Bet> Bets { get; set; } = null!;
    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
}
