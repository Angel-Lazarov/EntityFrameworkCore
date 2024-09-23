using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Customer")]
    public class CustomerDtoImport
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("bithDate")]
        public DateTime BirthDate { get; set; }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
