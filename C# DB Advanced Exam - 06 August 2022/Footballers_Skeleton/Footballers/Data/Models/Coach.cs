using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models;

public class Coach
{
    public int Id { get; set; }

    [MaxLength(40)]
    public string Name { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public virtual ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();
}