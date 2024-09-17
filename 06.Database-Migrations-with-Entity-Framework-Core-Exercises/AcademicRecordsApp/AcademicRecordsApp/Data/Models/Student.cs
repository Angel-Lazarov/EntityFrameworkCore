namespace AcademicRecordsApp.Data.Models
{
    public partial class Student
    {
        public Student()
        {
            Grades = new HashSet<Grade>();
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public int Age { get; set; }
        public decimal Celary { get; set; }
        public string Jobtitle { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }

    }
}
