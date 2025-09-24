using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAttendanceManager.Models;

namespace SchoolAttendanceManager.Views.view_Model;

public class UserRegisterVM
{
    public Registration UserReg { get; set; } = new();


    public IEnumerable<SelectListItem> RoleList { get; set; } = Enumerable.Empty<SelectListItem>();
}
