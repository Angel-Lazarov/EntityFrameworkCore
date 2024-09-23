using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellerDto
    {
        [Required]
        [MinLength(SellerNameMinLength)]
        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(SellerAddresMinLength)]
        [MaxLength(SellerAddresMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(@"^w{3}\.[a-zA-Z0-9-]+\.com$")]
        public string Website { get; set; } = null!;

        [JsonProperty(nameof(Boardgames))]
        public int[] Boardgames { get; set; } = null!;
    }
}
