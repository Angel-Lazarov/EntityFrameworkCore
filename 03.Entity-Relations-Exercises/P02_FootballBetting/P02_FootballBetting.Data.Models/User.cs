using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;
public class User
{
    public User()
    {
        Bets = new HashSet<Bet>();
    }
    [Key]
    public int UserId { get; set; }


    [Required]
    [MaxLength(ValidationConstants.UsernameMaxLength)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.UsersNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(ValidationConstants.PasswordMinLength)]
    [MaxLength(ValidationConstants.PasswordMaxLength)]
    public string Password { get; set; } = null!;

    [Column(TypeName = "VARCHAR")]
    [MaxLength(ValidationConstants.EmailMaxLength)]
    public string Email { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<Bet> Bets { get; set; }

}
