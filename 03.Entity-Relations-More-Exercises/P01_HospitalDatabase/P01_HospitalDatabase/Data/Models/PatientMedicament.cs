using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models;

public class PatientMedicament
{
    public int PatientId { get; set; }
    [ForeignKey(nameof(PatientId))]
    public virtual Patient Patient { get; set; }
    //*******************************************

    [ForeignKey(nameof(Medicament))]
    public int MedicamentId { get; set; }

    public Medicament Medicament { get; set; }

}
