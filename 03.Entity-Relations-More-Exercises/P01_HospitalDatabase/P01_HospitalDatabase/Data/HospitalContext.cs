using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {

        public DbSet<Diagnose> Diagnoses { get; set; }
        //public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }
        public DbSet<Visitation> Visitations { get; set; }


        //--------------------------------This is for JUDJE-----------------------------------//////

        public HospitalContext(DbContextOptions dbcontextoptions) : base(dbcontextoptions) { }

        //string connectionString = "Server=192.168.88.40 ,1434; Database = HospitalDatabase; User Id = sa; Password = password;";
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new { pm.PatientId, pm.MedicamentId });
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity
                .HasOne(d => d.Patient)
                .WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity
                .HasOne(v => v.Patient)
                .WithMany(p => p.Visitations)
                .HasForeignKey(v => v.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
            });

            //modelBuilder.Entity<Visitation>(entity =>
            //{
            //    entity
            //    .HasOne(v => v.Doctor)
            //    .WithMany(d => d.Visitations)
            //    .HasForeignKey(v => v.DoctorId)
            //    .OnDelete(DeleteBehavior.NoAction);
            //});

            //modelBuilder.Ignore<Doctor>();

        }
    }
}
