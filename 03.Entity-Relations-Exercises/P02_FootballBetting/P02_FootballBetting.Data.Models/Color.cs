﻿using P02_FootballBetting.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models;
public class Color
{
    public Color()
    {
        PrimaryKitTeams = new HashSet<Team>();
        SecondaryKitTeams = new HashSet<Team>();
    }

    [Key]
    public int ColorId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ColorNameMaxLength)]
    public string Name { get; set; } = null!;

    [InverseProperty(nameof(Team.PrimaryKitColor))]
    public virtual ICollection<Team> PrimaryKitTeams { get; set; } = null!;

    [InverseProperty(nameof(Team.SecondaryKitColor))]
    public virtual ICollection<Team> SecondaryKitTeams { get; set; } = null!;
}
