using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.Data.Models
{
    public class Seller
    {
        public int Id { get; set; }

        [MaxLength(SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(SellerAddresMaxLength)]
        public string Address { get; set; } = null!;

        public string Country { get; set; } = null!;


        public string Website { get; set; } = null!; //Validate with Regex

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new List<BoardgameSeller>();

    }
}
