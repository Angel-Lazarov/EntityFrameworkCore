using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models;

public class Diagnose
{
    public int DiagnoseId { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [MaxLength(250)]
    public string Comments { get; set; } = null!;

    public int PatientId { get; set; }

    public virtual Patient Patient { get; set; } = null!;

}
