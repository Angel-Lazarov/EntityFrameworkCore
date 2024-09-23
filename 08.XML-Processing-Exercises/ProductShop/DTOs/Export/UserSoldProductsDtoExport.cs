﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("User")]
    public class UserSoldProductsDtoExport
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("soldProducts")]
        public ProductSoldDtoExport[] SoldProducts { get; set; } = null!;

    }
}
