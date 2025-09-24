using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Models;

public partial class Registration
{
    public int Id { get; set; }

   
    public string? Role { get; set; }

    public string? Name { get; set; }

    [Required(ErrorMessage ="Email is Mandatory")]
    [EmailAddress(ErrorMessage ="valid Email is Required")]
    [StringLength(100,ErrorMessage ="Email Cant be longer than 100 Character")]
    public string? Email { get; set; }

    public string? Password { get; set; }
    [Required(ErrorMessage = "Role Name is Mandatory")]
    public int? RoleId { get; set; }

    public string? ImageUpload { get; set; }

    public bool? Isdeleted { get; set; }

    public DateOnly? RegistrationDate { get; set; }

  
    public virtual Role? RoleNavigation { get; set; }

    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();
}
