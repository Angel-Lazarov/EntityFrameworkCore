using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("Product")]
    public class ProductSoldDtoExport
    {
        [XmlElement("name")]
        public string ProductName { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
