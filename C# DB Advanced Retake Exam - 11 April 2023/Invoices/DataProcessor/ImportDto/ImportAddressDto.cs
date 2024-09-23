using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Address))]
    public class ImportAddressDto
    {
        [XmlElement(nameof(StreetName))]
        [Required]
        [MinLength(AddressStreetNameMinLength)]
        [MaxLength(AddressStreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [XmlElement(nameof(StreetNumber))]
        [Required]
        public int StreetNumber { get; set; }

        [XmlElement(nameof(PostCode))]
        [Required]
        public string PostCode { get; set; } = null!;

        [XmlElement(nameof(City))]
        [Required]
        [MinLength(AddressCityMinLength)]
        [MaxLength(AddressCityMaxLength)]
        public string City { get; set; } = null!;

        [XmlElement(nameof(Country))]
        [MinLength(AddressCountryMinLength)]
        [MaxLength(AddressCountryMaxLength)]
        public string Country { get; set; } = null!;

    }
}
