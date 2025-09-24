using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAttendanceManager.Models;

namespace SchoolAttendanceManager.Views.view_Model
{
    public class StudentDetailsVM
    {
        public StudentDetail studentDetailsReg { get; set; } = new();

        public IEnumerable<SelectListItem> NameList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> SubjectList { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> ResultList { get; set; } = Enumerable.Empty<SelectListItem>();

      
    }
}
