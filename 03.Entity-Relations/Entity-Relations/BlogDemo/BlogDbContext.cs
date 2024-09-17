using BlogDemo.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BlogDemo;

public class BlogDbContext : DbContext
{
    public BlogDbContext()
    {

    }

    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BlogConfiguration());
        //modelBuilder.Entity<Blog>()
        //    //.HasKey("BlogId");
        //    .HasKey(b => b.BlogId);

        //modelBuilder.Entity<Blog>()
        //    .ToTable("Blogs", "blg"); // даваме име на таблицата и схемата

        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.Name)
        //    .HasColumnName("BlogName")   //custom column name
        //    .HasColumnType("NVARCHAR")
        //    .HasMaxLength(50)
        //    .IsRequired();

        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.Description)
        //    .HasColumnType("NVARCHAR")
        //    .HasMaxLength(500);

        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.LastUpdated)
        //    .ValueGeneratedOnUpdate();

        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.Created)
        //    .ValueGeneratedOnAdd();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionstring = "Server=192.168.88.40 ,1434; Database = BlogDB; User Id = sa; Password = password;";
            optionsBuilder.UseSqlServer(connectionstring);
        }
    }

    public DbSet<Blog> Blogs { get; set; }
}
