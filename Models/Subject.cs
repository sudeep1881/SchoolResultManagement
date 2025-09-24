using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Models;

public partial class Subject
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Subject Name Is Mandaroty")]
    public string? SubjectName { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();
}
