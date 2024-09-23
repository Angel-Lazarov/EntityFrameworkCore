using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data;
public class FootballBettingContext : DbContext
{
    private const string ConnectionString = "Server =.;Database = FootballBookmakerSystem;User Id = sa;Password = SoftUn!2021; TrustServerCertificate=True;";



    public DbSet<Country> Countries { get; set; }
    public DbSet<Town> Towns { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Bet> Bets { get; set; }
    public DbSet<PlayerStatistic> PlayersStatistics { get; set; }

    //public FootballBettingContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    //{
    //}


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

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
            entity
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId);
        });

        //modelBuilder
        //    .Entity<Player>()
        //    .HasOne(p => p.)



    }
}
