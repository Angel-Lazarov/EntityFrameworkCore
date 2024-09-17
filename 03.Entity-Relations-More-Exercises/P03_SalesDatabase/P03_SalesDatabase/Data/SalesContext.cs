using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {

        string ConnectionString = "Server=192.168.88.40 ,1434; Database = SalesContext; User Id = sa; Password = password;";

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }


        ////--------------------------------This is for JUDJE-----------------------------------//////

        //public SalesContext(DbContextOptions dbcontextoptions) : base(dbcontextoptions) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(ConnectionString);

            //}
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(true);

                entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

                entity.Property(e => e.CreditCardNumber)
                .IsUnicode(false)
                .HasMaxLength(20);
            });

            builder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(true);
            });

            builder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(true);
            });

            builder.Entity<Sale>(entity =>
            {
                entity
                .HasOne(s => s.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

                entity
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(s => s.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.StoreId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
