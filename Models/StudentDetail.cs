using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Models;

public partial class StudentDetail
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Student Name is Required ")]
    public int? StudentId { get; set; }

    public string? Class { get; set; }

    public string? Section { get; set; }
    [Required(ErrorMessage = "subject Name is Required")]
    public int? SubjectsId { get; set; }

    public string? Marks { get; set; }

    public string? Percentage { get; set; }

    [Required(ErrorMessage = "Result Name is required")]
    public int? ResultId { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual Result? Result { get; set; }

    public virtual Registration? Registration { get; set; }

    public virtual Subject? Subjects { get; set; }
}
