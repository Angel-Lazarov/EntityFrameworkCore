using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType(nameof(Despatcher))]
    public class ImportDespetcherDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Position))]
        [Required]
        public string? Position { get; set; }

        [XmlArray(nameof(Trucks))]
        [Required]
        public ImportTruckDto[] Trucks { get; set; } = null!;
    }
}
