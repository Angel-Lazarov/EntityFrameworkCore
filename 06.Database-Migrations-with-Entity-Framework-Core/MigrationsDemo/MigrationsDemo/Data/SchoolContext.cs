using Microsoft.EntityFrameworkCore;
using MigrationsDemo.Models;

namespace MigrationsDemo.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=School;User Id = sa;Password = SoftUn!2021; TrustServerCertificate=True;");
        }
    }
}
