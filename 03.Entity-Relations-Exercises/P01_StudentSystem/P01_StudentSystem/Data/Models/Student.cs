using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MinLength(10)]
    [MaxLength(10)]
    [Column(TypeName = "VARCHAR")]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }
    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
}
