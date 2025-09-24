using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SchoolAttendanceManager.DTOs;
using SchoolAttendanceManager.Models;



namespace SchoolAttendanceManager.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolAttendanceDbContext _db;

        public StudentController(SchoolAttendanceDbContext db)
        {
            _db = db;
        }

        #region--Student Result------

        #region---Get Method------------
        public IActionResult StudentList()
        {
            return View();
        }
        #endregion

        #region ---Fetch Method----
        [HttpPost]
        public async Task<IActionResult> FetchMethod()
        {
            // get the logged-in student's RegistrationId from session
            int? regid = HttpContext.Session.GetInt32(SD.KeyUser);

            if (regid == null)
            {
                return Json(new { data = new List<StudentDetailsDTOs>() });// not logged in
            } 

            // fetch only records of this student
            var studentresult = await _db.StudentDetails
                .AsNoTracking()
                .Where(s => s.Isdeleted == false && s.StudentId == regid) // filter here
                .Include(s => s.Registration)
                .Include(s => s.Result)
                .Include(s => s.Subjects)
                .ToListAsync();

            var result = studentresult.Select(s => new StudentDetailsDTOs
            {
                IdDTO = s.Id,
                StudentNameDTO = s.Registration!.Name,
                ClassDTO = s.Class,
                SectionDTO = s.Section,
                SubjectNameDTO = s.Subjects!.SubjectName,
                MarksDTO = s.Marks,
                PercentageDTO = s.Percentage,
                ResultNameDTO = s.Result!.ExamResult
            }).ToList();

            return Json(new { data = result });
        }
        #endregion
        #endregion
    }
}
