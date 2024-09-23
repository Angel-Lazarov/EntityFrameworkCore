using System.Net;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
    private const string ConnectionString = "Server =.;Database = StudentSystem;User Id = sa;Password = SoftUn!2021; TrustServerCertificate=True;";

    public StudentSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Homework> Homeworks { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(ConnectionString);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the composite primary key (StudentId & CourseId)
        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<Student>()
            .Property(s => s.PhoneNumber)
            .IsUnicode(false);
    }
}

