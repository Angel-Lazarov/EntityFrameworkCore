using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace P01_StudentSystem.Data.Models;
public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Required]
    [MaxLength(50)]
    //[Column(TypeName = "NVARCHAR")] // nvarchar by default!
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "VARCHAR")] // nvarchar by default!
    public string Url { get; set; }

    public ResourceType ResourceType { get; set; }


    //-----------------------------------------------------------
    public int CourseId { get; set; } // Foreign Key
    
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } // navigation property

    // need to set a collection of resources in the Course model.
    //-----------------------------------------------------------
}

public enum ResourceType
{
    Video,
    Presentation,
    Document,
    Other
}
