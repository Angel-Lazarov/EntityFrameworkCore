using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [MaxLength(80)]
    //[Column(TypeName = "NVARCHAR")] // nvarchar by default!
    [Required]
    public string Name { get; set; } = null!;

    //[Column(TypeName = "NVARCHAR")] // nvarchar by default!
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    //[Column(TypeName = "DECIMAL(18,2)")]
    public decimal Price { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    public virtual ICollection<Resource> Resources { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }
}

