using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Creator))]
    public class ImportCreatorDto
    {
        [XmlElement(nameof(FirstName))]
        [Required]
        [MinLength(CreatorFirstNameMinLengtht)]
        [MaxLength(CreatorFirstNameMaxLengtht)]
        public string FirstName { get; set; } = null!;

        [XmlElement(nameof(LastName))]
        [Required]
        [MinLength(CreatorLastNameMinLengtht)]
        [MaxLength(CreatorLastNameMaxLengtht)]
        public string LastName { get; set; } = null!;

        [XmlArray(nameof(Boardgames))]
        [Required]
        public ImportBoardgameDto[] Boardgames { get; set; } = null!;

    }
}
