﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class CarDtoWithPartsExport
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public PartDtoExport[] PartDtoExports { get; set; }

    }
}
