using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("partId")]
    public class PartCarDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
