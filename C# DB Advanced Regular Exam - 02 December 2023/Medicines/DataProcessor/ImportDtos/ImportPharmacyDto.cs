using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Pharmacy))]
    public class ImportPharmacyDto
    {
        [XmlAttribute("non-stop")]
        [Required]
        public string NonStop { get; set; }

        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(PhoneNumber))]
        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"^\(\d{3}\)\s{1}\d{3}-\d{4}$")]
        //[RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$")]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray(nameof(Medicines))]
        public ImportMedicineDto[] Medicines { get; set; } = null!;
    }
}
