using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(14)]
        public string PhoneNumber { get; set; } = null!;

        public bool IsNonStop { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
