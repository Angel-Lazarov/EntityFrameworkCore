using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext


{
    //public StudentSystemContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    //{
    //}

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentCourse> StudentsCourses { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Homework> Homeworks { get; set; }


    string connectionstring = "Server=192.168.88.40 ,1434; Database = StudentSystem; User Id = sa; Password = password;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionstring);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentCourse>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });
    }
}