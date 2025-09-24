using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Models;

public partial class Role
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Role Name is  Required")]
    public string? Name { get; set; }

    public bool? Isdeleted { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
