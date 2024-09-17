using AcademicRecordsApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicRecordsApp.Data
{
    public partial class AcademicRecordsDBContext : DbContext
    {
        public AcademicRecordsDBContext()
        {
        }

        public AcademicRecordsDBContext(DbContextOptions<AcademicRecordsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.88.40,1434;Database=AcademicRecordsDB;User Id=sa; Password=password;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.Courses)
                    .UsingEntity<Dictionary<string, object>>(
                        "StudentsCourse",
                        l => l.HasOne<Student>().WithMany().HasForeignKey("StudentsId"),
                        r => r.HasOne<Course>().WithMany().HasForeignKey("CoursesId"),
                        j =>
                        {
                            j.HasKey("CoursesId", "StudentsId");

                            j.ToTable("StudentsCourses");

                            j.HasIndex(new[] { "StudentsId" }, "IX_StudentsCourses_StudentsId");
                        });
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasIndex(e => e.ExamId, "IX_Grades_ExamId");

                entity.HasIndex(e => e.StudentId, "IX_Grades_StudentId");

                entity.Property(e => e.Value).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.ExamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grades_Exams");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grades_Students");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.FullName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
