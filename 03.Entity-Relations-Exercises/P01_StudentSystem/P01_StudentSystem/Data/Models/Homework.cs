﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class Homework
{
    [Key]
    public int HomeworkId { get; set; }


    [Column(TypeName = "VARCHAR")]
    public string? Content { get; set; }

    public ContentType ContentType { get; set; }

    [Required]
    public DateTime SubmissionTime { get; set; }

    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; }


    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; }

}
