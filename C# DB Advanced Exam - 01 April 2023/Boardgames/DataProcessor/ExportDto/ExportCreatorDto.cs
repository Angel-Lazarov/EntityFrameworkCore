using Boardgames.Data.Models;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType(nameof(Creator))]
    public class ExportCreatorDto
    {
        [XmlAttribute(nameof(BoardgamesCount))]
        public int BoardgamesCount { get; set; }

        [XmlElement(nameof(CreatorName))]
        public string CreatorName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        public ExportBoardgameDto[] Boardgames { get; set; } = null!;
    }
}
