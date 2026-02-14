
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAttendanceManager.DTOs;
using SchoolAttendanceManager.Models;
using SchoolAttendanceManager.Views.view_Model;


namespace SchoolAttendanceManager.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly SchoolAttendanceDbContext _db;

        public TeacherController(SchoolAttendanceDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;

        }

        #region--DDL(Registration Drop Down)
        private async Task<IEnumerable<SelectListItem>> Getregister()
        {
            var registrations = _db.Registrations.Where(r => r.Isdeleted == false && r.RoleId == 2);
            return registrations.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            }).ToList();
        }

        #endregion




        #region---DDL(Subject DROP DOWN)
        private async Task<IEnumerable<SelectListItem>> GetSubject()
        {
            var subjects = await _db.Subjects.Where(a => a.Isdeleted == false).ToListAsync();
            return subjects.Select(s => new SelectListItem
            {
                Text = s.SubjectName,
                Value = s.Id.ToString()
            }).ToList();
        }
        #endregion



        #region---DDL(result DROP DOWN)
        private async Task<IEnumerable<SelectListItem>> GetResult()
        {
            var results = _db.Results.Where(a => a.Isdeleted == false);
            return results.Select(s => new SelectListItem
            {
                Text = s.ExamResult,
                Value = s.Id.ToString()
            }).ToList();
        }
        #endregion

        #region----Dashboard--------
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion


       #region---Student Result Registaration---

        #region---Get Method----
        public async Task<IActionResult> StudentDetails(int? id)
        {
            Console.WriteLine("Hello World ");
            var studentDetailsvm = new StudentDetailsVM()
            {

                NameList = await Getregister(),
                SubjectList = await GetSubject(),
                ResultList = await GetResult()
            };
            if (id.HasValue && id != 0)
            {
                var role = _db.StudentDetails.Where(r => r.Isdeleted == false && r.Id == id).FirstOrDefault();
                if (role == null)
                {

                    TempData["error"] = "Data Not Found";
                    return RedirectToAction();
                }
                studentDetailsvm.studentDetailsReg = role;
            }
            return View(studentDetailsvm);
        }
        #endregion

        #region---Post Method-----
        [HttpPost]
        public IActionResult StudentDetails(StudentDetailsVM studentvm)
        {

            studentvm.studentDetailsReg.Isdeleted = false;
            if (studentvm.studentDetailsReg.Id == 0)
            {
                _db.StudentDetails.Add(studentvm.studentDetailsReg);
            }
            else
            {
                _db.StudentDetails.Update(studentvm.studentDetailsReg);
            }
            _db.SaveChanges();
            TempData["success"] = "Data Saved Successfully";
            return RedirectToAction();
        }
        #endregion

        //#region---Fetch Method-----
        //public IActionResult FetchMethodSample()
        //{
        //    var datalist = _db.StudentDetails.Where(s => s.Isdeleted == false).ToList();
        //    return Json(new { data = datalist });
        //}


        #region---Fetch Method (API)-----
        [HttpPost]
        public async Task<IActionResult> StudentDetailsFetchMethod()
        {
            var passstudent = await _db.StudentDetails.AsNoTracking().
           Where(s => s.Isdeleted == false).Include(s => s.Registration).Include(s => s.Result).Include(s => s.Subjects).ToListAsync();
            var result = passstudent.Select(s => new StudentDetailsDTOs
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

        //#region------Advance Search Method------
        //[HttpPost]
        //public IActionResult AdvanceSearchMathod()
        //{
        //     var studentdetails=_db.StudentDetails.Where(s=>s.Isdeleted)
        //}
        //#endregion

        #region------advance Search Form-----
        [HttpPost]
        public IActionResult AdvnaceSearchFormStudentDetails(int? studentNameId, string? studentclass, string? section, int? result)
        {
            var passstudent = _db.StudentDetails.Where(s => s.Isdeleted == false
           && ((studentNameId > 0) ? s.StudentId == studentNameId : true)
           && ((studentclass != null) ? s.Class == studentclass : true)
           && ((section != null) ? s.Section == section : true)
           && ((result > 0) ? s.ResultId == result : true)).Select(s => new StudentDetailsDTOs
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
            return Json(new { data = passstudent });
        }

        #endregion

        #region---Delete Method-----
        [HttpDelete]
        public IActionResult StudentDetailsDeleteMethod(int id)
        {
            var studentDetail = _db.StudentDetails.FirstOrDefault(s => s.Isdeleted == false && s.Id == id);
            if (studentDetail == null)
            {
                return Json(new { success = false, message = "Data Not Found" });
            }
            studentDetail.Isdeleted = true;
            _db.StudentDetails.Update(studentDetail);
            _db.SaveChanges();
            return Json(new { success = true, message = "Data Delete Successfully" });
        }
        #endregion


        #endregion

        #region----Result  Details------

        #region--Get Method---
        public async Task<IActionResult> StudentListFetching()
        {
            var studentdetailsvm = new StudentDetailsVM()
            {
                NameList = await Getregister(),
                ResultList = await GetResult()
            };
            return View(studentdetailsvm);
        }
        #endregion

        #region---Fetch Method-----

        [HttpPost]
        public async Task<IActionResult> StudentFetchingList()
        {
            var passstudent = await _db.StudentDetails.AsNoTracking().
           Where(s => s.Isdeleted == false).Include(s => s.Registration).Include(s => s.Result).Include(s => s.Subjects).ToListAsync();
            var result = passstudent.Select(s => new StudentDetailsDTOs
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

        #region----Advance Fetch Method----
        [HttpPost]
        public IActionResult ResultDetailsAdvnaceFetchMethod(int? StudentName, string? StudentClass, string? section, int? result)
        {
            var studentDetails = _db.StudentDetails.Where(s => s.Isdeleted == false
                  && ((StudentName != 0) ? s.StudentId == StudentName : true)
                  && ((StudentClass != null) ? s.Class == StudentClass : true)
                  && ((section != null) ? s.Section == section : true)
                  && ((result != 0) ? s.ResultId == result : true)).Select(s => new StudentDetailsDTOs
                  {
                        IdDTO=s.Id,
                        StudentNameDTO=s.Registration!.Name,
                        ClassDTO=s.Class,
                        SectionDTO=s.Section,
                        SubjectNameDTO=s.Subjects!.SubjectName,
                        MarksDTO=s.Marks,
                        PercentageDTO=s.Percentage,
                        ResultNameDTO=s.Result!.ExamResult
                  }).ToList();

            return Json(new { data = studentDetails });
        }
        #endregion

        #region---Delete Method-----
        [HttpDelete]
        public IActionResult StudentListDeleteMethod(int id)
        {
            var studentDetail = _db.StudentDetails.FirstOrDefault(s => s.Isdeleted == false && s.Id == id);
            if (studentDetail == null)
            {
                return Json(new { success = false, message = "Data Not Found" });
            }
            studentDetail.Isdeleted = true;
            _db.StudentDetails.Update(studentDetail);
            _db.SaveChanges();
            return Json(new { success = true, message = "Data Delete Successfully" });
        }
        #endregion

        #endregion

        #region--Pass Student fecth Details--

        #region--Get Method----
        public IActionResult PassStudents(int? id)
        {
            return View();
        }
        #endregion

        //#region--Fetch Method----
        //[HttpPost]
        //public IActionResult PassStudentListFetchmethod()
        //{
        //    var DataList = _db.StudentDetails.Where(s => s.Isdeleted == false && s.ResultId == 1).ToList();
        //    return Json(new { data = DataList });
        //}
        //#endregion

        #region--Fetch Method----
        [HttpPost]
        public async Task<IActionResult> PassStudentFetching()
        {
            var passstudent = await _db.StudentDetails.AsNoTracking().
                Where(s => s.Isdeleted == false && s.ResultId == 1).Include(s => s.Registration).Include(s => s.Result).Include(s => s.Subjects).ToListAsync();
            var result = passstudent.Select(s => new StudentDetailsDTOs
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

        #region---Delete Method-----
        [HttpDelete]
        public IActionResult DeletePassStudent(int id)
        {
            var studentDetail = _db.StudentDetails.FirstOrDefault(s => s.Isdeleted == false && s.Id == id);
            if (studentDetail == null)
            {
                return Json(new { success = false, message = "Data Not Found" });
            }
            studentDetail.Isdeleted = true;
            _db.StudentDetails.Update(studentDetail);
            _db.SaveChanges();
            return Json(new { success = true, message = "Data Delete Successfully" });
        }
        #endregion


        #region--Download Excel JSON Way--
        [HttpPost]
        public async Task<IActionResult> PassStudentDownload()
        {
            var passstudent = await _db.StudentDetails.AsNoTracking()
                .Where(s => s.Isdeleted == false && s.ResultId == 1)
                .Include(s => s.Registration)
                .Include(s => s.Result)
                .Include(s => s.Subjects)
                .ToListAsync();

            var result = passstudent.Select(s => new StudentDetailsDTOs
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

            var returnObj = new
            {
                downloadAllow = 1,   // ✅ your sir's way
                data = new { result }
            };

            return new JsonResult(returnObj);
        }
        #endregion

        #endregion

        #region----Fail Student Details----

        #region---Get Method----
        public IActionResult FailStudents(int? id)
        {
            return View();
        }
        #endregion

        //#region---Fetch Method----
        //[HttpPost]
        //public IActionResult FailFetchMethod()
        //{
        //    var dataList = _db.StudentDetails.Where(s => s.Isdeleted == false&& s.ResultId==2).ToList();
        //    return Json(new { data = dataList });
        //}
        //#endregion

        #region---Fetch Method(API METHOD)----
        [HttpPost]
        public async Task<IActionResult> FailStudentFetching()
        {
            var failstudentList = await _db.StudentDetails.AsNoTracking().
                Where(s => s.Isdeleted == false && s.ResultId == 2).Include(s => s.Registration).
                Include(s => s.Subjects).Include(s => s.Result).ToListAsync();
            var result = failstudentList.Select(s => new StudentDetailsDTOs
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

        #region--Delete Method---
        [HttpDelete]
        public IActionResult FailDeleteStudentHandler(int id)
        {
            var role = _db.StudentDetails.FirstOrDefault(s => s.Isdeleted == false && s.Id == id);
            if (role == null)
            {
                return Json(new { success = false, message = "Data Not Found" });
            }
            role.Isdeleted = true;
            _db.StudentDetails.Update(role);
            _db.SaveChanges();
            return Json(new { success = true, message = "Data Delete Successfully" });
        }
        #endregion

        #region--Download Excel JSON Way--
        [HttpPost]
        public async Task<IActionResult> FailStudentDownload()
        {
            var passstudent = await _db.StudentDetails.AsNoTracking()
                .Where(s => s.Isdeleted == false && s.ResultId == 2)
                .Include(s => s.Registration)
                .Include(s => s.Result)
                .Include(s => s.Subjects)
                .ToListAsync();

            var result = passstudent.Select(s => new StudentDetailsDTOs
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

            var returnObj = new
            {
                downloadAllow = 1,   
                data = new { result }
            };
            return new JsonResult(returnObj);
        }
        #endregion

        #endregion



    }
}
