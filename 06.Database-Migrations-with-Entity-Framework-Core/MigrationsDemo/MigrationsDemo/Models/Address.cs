using System.ComponentModel.DataAnnotations;

namespace MigrationsDemo.Models
{
    public class Address
    {
        public Address()
        {
            Students = new HashSet<Student>();
        }
        public int Id { get; set; }

        [MaxLength(50)]
        public string Town { get; set; } = null!;

        [MaxLength(50)]
        public string Text { get; set; } = null!;

        public ICollection<Student> Students { get; set; }
    }
}
