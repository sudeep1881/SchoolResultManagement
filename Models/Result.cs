using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Models;

public partial class Result
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Name is Mandatory")]
    public string? ExamResult { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();
}
