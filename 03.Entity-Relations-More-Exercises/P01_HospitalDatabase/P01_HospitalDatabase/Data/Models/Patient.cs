using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models;

public class Patient
{
    public Patient()
    {
        Prescriptions = new HashSet<PatientMedicament>();
        Diagnoses = new HashSet<Diagnose>();
        Visitations = new HashSet<Visitation>();
    }
    public int PatientId { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [MaxLength(250)]
    public string Address { get; set; } = null!;

    [MaxLength(80)]
    [Column(TypeName = "VARCHAR")]
    public string Email { get; set; }

    public bool HasInsurance { get; set; }

    public ICollection<PatientMedicament> Prescriptions { get; set; }

    public ICollection<Diagnose> Diagnoses { get; set; }
    public ICollection<Visitation> Visitations { get; set; }

}
