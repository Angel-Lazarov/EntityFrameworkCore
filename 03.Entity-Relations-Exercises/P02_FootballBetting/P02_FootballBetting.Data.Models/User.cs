using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class User
{
    public User()
    {
        Bets = new HashSet<Bet>();
    }
    public int UserId { get; set; }

    [MaxLength(80)]
    public string Username { get; set; } = null!;

    [MaxLength(80)]
    public string Name { get; set; } = null!;

    [MaxLength(50)]
    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public decimal Balance { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = null!;
}
