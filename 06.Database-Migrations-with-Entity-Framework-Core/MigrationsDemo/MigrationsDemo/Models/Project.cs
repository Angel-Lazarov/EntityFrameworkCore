using System.ComponentModel.DataAnnotations;

namespace MigrationsDemo.Models
{
    public class Project
    {
        public Project()
        {
            Students = new HashSet<Student>();
        }
        public int Id { get; set; }

        [MaxLength(50)]
        public string ProjectName { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public ICollection<Student> Students { get; set; }

        //public int Test1 { get; set; }

        //public int Test2 { get; set; }

        //public int Test3 { get; set; }

    }
}
