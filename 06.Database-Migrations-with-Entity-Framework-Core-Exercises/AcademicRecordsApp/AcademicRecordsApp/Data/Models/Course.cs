using System;
using System.Collections.Generic;

namespace AcademicRecordsApp.Data.Models
{
    public partial class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Student> Students { get; set; }
    }
}
