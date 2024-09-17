using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [MaxLength(80)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Resource> Resources { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

}
