using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Medicines.Data.DataConstrains;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType(nameof(Medicine))]
public class ImportMedicineDto
{
    [XmlAttribute("category")]
    [Required]
    [Range(0, 4)]
    public int Category { get; set; }

    [XmlElement(nameof(Name))]
    [Required]
    [MinLength(3)]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [XmlElement(nameof(Price))]
    [Required]
    [Range(typeof(decimal), MedicinePriceMinValue, MedicinePriceMaxValue)]
    public decimal Price { get; set; }

    [XmlElement(nameof(ProductionDate))]
    [Required]
    public string ProductionDate { get; set; } = null!;

    [XmlElement(nameof(ExpiryDate))]
    [Required]
    public string ExpiryDate { get; set; } = null!;

    [XmlElement(nameof(Producer))]
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Producer { get; set; } = null!;
}