using System;
using System.Collections.Generic;

namespace SchoolAttendanceManager.Models;

public partial class UsersIdentity
{
    public int Id { get; set; }

    public string? Role { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public int? RoleId { get; set; }

    public bool? Isdeleted { get; set; }

    //public virtual RolesIdentity? RoleNavigation { get; set; }
}
