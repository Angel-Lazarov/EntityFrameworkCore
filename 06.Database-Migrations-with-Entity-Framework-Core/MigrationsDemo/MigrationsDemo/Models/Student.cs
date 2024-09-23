using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigrationsDemo.Models
{
    public class Student
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; } = null!;

        public int Age { get; set; }

        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
    }
}
