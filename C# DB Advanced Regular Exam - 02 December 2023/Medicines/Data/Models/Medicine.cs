using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models;

public class Medicine
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public Category Category { get; set; }

    public DateTime ProductionDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    [MaxLength(100)]
    public string Producer { get; set; } = null!;

    public int PharmacyId { get; set; }

    public virtual Pharmacy Pharmacy { get; set; } = null!;

    public virtual ICollection<PatientMedicine> PatientsMedicines { get; set; } = new List<PatientMedicine>();
}