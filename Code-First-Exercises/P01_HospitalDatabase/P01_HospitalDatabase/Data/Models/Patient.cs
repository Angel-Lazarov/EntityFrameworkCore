using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Visitations = new HashSet<Visitation>();
            Diagnoses = new HashSet<Diagnose>();
            Prescriptions = new HashSet<PatientMedicament>();
        }
        public int PatientId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(250)]
        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;
        public bool HasInsurance { get; set; }

        public virtual ICollection<Visitation>? Visitations { get; set; }

        public virtual ICollection<Diagnose>? Diagnoses { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
