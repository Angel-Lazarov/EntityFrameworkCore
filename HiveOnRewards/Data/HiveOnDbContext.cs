using Microsoft.EntityFrameworkCore;

namespace HiveOnRewards.Data
{
    public class HiveOnDbContext : DbContext
    {
        string connectionString = "Server=192.168.88.40 ,1434; Database = HiveOnDb; User Id = sa; Password = password;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
