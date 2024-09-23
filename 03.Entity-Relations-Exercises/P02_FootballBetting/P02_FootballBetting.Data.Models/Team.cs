using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;
public class Team
{
    public Team()
    {
        Players = new HashSet<Player>();
        HomeGames = new HashSet<Game>();
        AwayGames = new HashSet<Game>();
    }

    [Key]
    public int TeamId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TeamNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ValidationConstants.LogoUrlMaxLength)]
    public string? LogoUrl { get; set; }

    [MaxLength(ValidationConstants.InitialsMaxLength)]
    public string Initials { get; set; } = null!;

    public decimal Budget { get; set; }
    //--------------------------------------------

    public int PrimaryKitColorId { get; set; }

    [ForeignKey(nameof(PrimaryKitColorId))]
    public virtual Color PrimaryKitColor { get; set; } = null!;

    //--------------------------------------------

    //--------------------------------------------
    public int SecondaryKitColorId { get; set; }

    [ForeignKey(nameof(SecondaryKitColorId))]
    public virtual Color SecondaryKitColor { get; set; } = null!;
    //--------------------------------------------

    //--------------------------------------------
    public int TownId { get; set; }

    [ForeignKey(nameof(TownId))]
    public Town Town { get; set; } = null!;
    //--------------------------------------------

    public virtual ICollection<Player> Players { get; set; }

    [InverseProperty(nameof(Game.HomeTeam))]
    public virtual ICollection<Game> HomeGames { get; set; }

    [InverseProperty(nameof(Game.AwayTeam))]
    public virtual ICollection<Game> AwayGames { get; set; }
}
