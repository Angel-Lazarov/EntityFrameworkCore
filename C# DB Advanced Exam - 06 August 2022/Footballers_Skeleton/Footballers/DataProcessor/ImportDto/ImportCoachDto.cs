using Footballers.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType(nameof(Coach))]
    public class ImportCoachDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Nationality))]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray(nameof(Footballers))]
        [Required]
        public ImportFootballerDto[] Footballers { get; set; }
    }
}
