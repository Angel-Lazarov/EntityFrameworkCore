using Footballers.Data.Models;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType(nameof(Coach))]
    public class ExportCoachDto
    {
        [XmlAttribute(nameof(FootballersCount))]
        public int FootballersCount { get; set; }

        [XmlElement(nameof(CoachName))]
        public string CoachName { get; set; } = null!;

        [XmlArray(nameof(Footballers))]
        public ExportFootballerDto[] Footballers { get; set; } = null!;
    }
}
