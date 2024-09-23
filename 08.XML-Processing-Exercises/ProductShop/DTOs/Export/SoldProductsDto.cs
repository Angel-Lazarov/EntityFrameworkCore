using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("SoldProducts")]
    public class SoldProductsDto
    {
        [XmlElement("count")]
        public int SoldProductsCount { get; set; }

        [XmlArray("products")]
        public ProductNamePriceDtoExport[] Products { get; set; }

    }
}
