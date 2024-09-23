using System.ComponentModel.DataAnnotations;
using static Boardgames.Data.DataConstraints;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        public int Id { get; set; }

        [MaxLength(CreatorFirstNameMaxLengtht)]
        public string FirstName { get; set; } = null!;

        [MaxLength(CreatorLastNameMaxLengtht)]
        public string LastName { get; set; } = null!;

        public virtual ICollection<Boardgame> Boardgames { get; set; } = new List<Boardgame>();
    }

}
