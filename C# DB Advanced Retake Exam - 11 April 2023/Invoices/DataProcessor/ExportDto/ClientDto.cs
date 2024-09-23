using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ClientDto
    {
        [XmlAttribute("InvoicesCount")]
        public int InvoicesCount { get; set; }

        [XmlElement("ClientName")]
        public string Name { get; set; } = null!;

        [XmlElement("VatNumber")]
        public string VatNumber { get; set; } = null!;

        [XmlArray("Invoices")]
        public InvoiceDto[] Invoices { get; set; } = null!;
    }
}
