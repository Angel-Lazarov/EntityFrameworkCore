using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data;

public class FootballBettingContext : DbContext
{
    string connectionString = "Server=192.168.88.40 ,1434; Database = FootballBookmakerSystem; User Id = sa; Password = password;";

    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<Town> Towns { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Bet> Bets { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;


    //--------------------------------This is for JUDJE-----------------------------------//////
    public FootballBettingContext(DbContextOptions dbcontextoptions) : base(dbcontextoptions)
    {
    }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{

    //    optionsBuilder.UseSqlServer(connectionString);

    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity
                .HasKey(ps => new { ps.PlayerId, ps.GameId });
        });

        modelBuilder.Entity<Team>(entity =>
          {
              entity
              .HasOne(t => t.PrimaryKitColor)
              .WithMany(c => c.PrimaryKitTeams)
              .HasForeignKey(t => t.PrimaryKitColorId)
              .OnDelete(DeleteBehavior.NoAction);
          });

        modelBuilder.Entity<Team>(entity =>
        {
            entity
            .HasOne(t => t.SecondaryKitColor)
            .WithMany(c => c.SecondaryKitTeams)
            .HasForeignKey(t => t.SecondaryKitColorId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity
            .HasOne(g => g.HomeTeam)
            .WithMany(t => t.HomeGames)
            .HasForeignKey(g => g.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasOne(g => g.AwayTeam)
            .WithMany(t => t.AwayGames)
            .HasForeignKey(g => g.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity
            .HasOne(p => p.Position)
            .WithMany(p => p.Players)
            .HasForeignKey(p => p.PositionId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasOne(p => p.Town)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TownId)
            .OnDelete(DeleteBehavior.NoAction);
        });
    }


}
