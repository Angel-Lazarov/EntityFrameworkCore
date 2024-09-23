using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        //[RegularExpression(@"^[A-Za-z0-9\s\.\-]{3,}$")]
        [RegularExpression(@"^[A-Za-z0-9\s\.\-]+$")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }

        [Required]
        [JsonProperty(nameof(Footballers))]
        public int[] FootballersIds { get; set; } = null!;
    }
}
