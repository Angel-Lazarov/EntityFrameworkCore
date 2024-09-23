using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class BmwDto_Export
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }
    }
}
