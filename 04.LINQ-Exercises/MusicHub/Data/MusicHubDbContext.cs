using MusicHub.Data.Models;

namespace MusicHub.Data;
using Microsoft.EntityFrameworkCore;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    {
    }

    public MusicHubDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Performer> Performers { get; set; } = null!;
    public DbSet<Producer> Producers { get; set; } = null!;
    public DbSet<Song> Songs { get; set; } = null!;
    public DbSet<Writer> Writers { get; set; } = null!;
    public DbSet<SongPerformer> SongsPerformers { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(Configuration.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Album>(entity =>
        {
            entity.Property(a => a.ReleaseDate)
                .HasColumnType("date");
        });

        builder.Entity<Album>(entity =>
        {
            entity
                .HasOne(a => a.Producer)
                .WithMany(p => p.Albums)
                .HasForeignKey(a => a.ProducerId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Song>(entity =>
        {
            entity.Property(s => s.CreatedOn)
                .HasColumnType("date");
        });

        builder.Entity<Song>(entity =>
        {
            entity
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Song>(entity =>
        {
            entity.HasOne(s => s.Writer)
                .WithMany(w => w.Songs)
                .HasForeignKey(s => s.WriterId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<SongPerformer>(entity =>
        {
            entity
                .HasKey(sp => new { sp.SongId, sp.PerformerId });
        });

    }
}
