namespace TestProject.Data
{
    using Microsoft.EntityFrameworkCore;
    using TestProject.Data.Models;

    public class TestProjectContext : DbContext
    {
        public TestProjectContext()
        {
        }

        public TestProjectContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}