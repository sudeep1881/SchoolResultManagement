using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAttendanceManager.DTOs;
using SchoolAttendanceManager.Image_Services;
using SchoolAttendanceManager.Models;
using SchoolAttendanceManager.Views.view_Model;


namespace SchoolAttendanceManager.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly SchoolAttendanceDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AdminController(SchoolAttendanceDbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
    }

    #region--DDL(Role DropDown List)--
    private IEnumerable<SelectListItem> GetRoles()
    {
        var roles =  _db.Roles.Where(a => a.Isdeleted == false);
        return roles.Select(r => new SelectListItem
        {
            Text = r.Name,
            Value = r.Id.ToString()
        }).ToList();
    }


    #endregion

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
        var subjects = _db.Subjects.Where(a => a.Isdeleted == false);
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

    #region--Dashboard--
    public IActionResult Dashboard()
    {
        return View();
    }
    #endregion


    #region---Role----

    #region--Get Method------
    public IActionResult Role(int? id)
    {
        var rolevm = new RoleVM();
        if (id.HasValue && id != 0)
        {
            var rode = _db.Roles.Where(u => u.Isdeleted == false && u.Id == id).FirstOrDefault();
            if (rode == null)
            {
                TempData["error"] = "Data Not Found";
                return RedirectToAction();
            }
            rolevm.roleReg = rode;
        }

        return View(rolevm);


    }
    #endregion


    #region----Post Method-------
    [HttpPost]
    public IActionResult Role(RoleVM rolevm)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = " Role Name  is Mandatroy";
            return View(rolevm);
        }
        rolevm.roleReg.Isdeleted = false;
        if (rolevm.roleReg.Id == 0)
        {
            _db.Roles.Add(rolevm.roleReg);
        }
        else
        {
            _db.Roles.Update(rolevm.roleReg);
        }
        _db.SaveChanges();
        TempData["success"] = "Data saved Successfully";
        return RedirectToAction();
    }
    #endregion

    #region-----Fetch Method----
    [HttpPost]
    public IActionResult RoleFetchMethod()
    {
        var dataList = _db.Roles.Where(r => r.Isdeleted == false).ToList();
        return Json(new { data = dataList });
    }

    #endregion

    #region----Delete Method-------
    [HttpDelete]
    public IActionResult RoleDeleteHandler(int id)
    {
        var rode = _db.Roles.FirstOrDefault(r => r.Isdeleted == false && r.Id == id);
        if (rode == null)
        {
            return Json(new { success = false, message = "Data Not Found" });

        }
        rode.Isdeleted = true;
        _db.Roles.Update(rode);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Deleted" });
    }
    #endregion

    #region---Excel Download format----
    [HttpPost]
    public async Task<IActionResult> RoleExecldownload()
    {
        var role = await _db.Roles.AsNoTracking().Where(s => s.Isdeleted == false).ToListAsync();
        var result = role.Select(s => new Role
        {
            Id = s.Id,
            Name = s.Name
        }).ToList();

        var roleobj = new
        {
            downloadAllow = 1,
            data = new { result }
        };
        return new JsonResult(roleobj);
    }
    #endregion

    #endregion


    #region--Registration--

    #region--Get Method---
    public async Task<IActionResult> Registration(int? id)
    {
        var userRegvm = new UserRegisterVM
        {
            RoleList =  GetRoles()

        };
        if (id.HasValue && id != 0)
        {
            var rode = _db.Registrations.Where(u => u.Isdeleted == false && u.Id == id).FirstOrDefault();
            if (rode == null)
            {
                TempData["error"] = "Data Not Found";
                return RedirectToAction();
            }
            userRegvm.UserReg = rode;
        }

        return View(userRegvm);
    }
    #endregion

    #region---Post Method---
    [HttpPost]
    public async Task<IActionResult> Registration(UserRegisterVM userVM, IFormFile fileProfileImage)
    {
        if (!ModelState.IsValid)
        {
            userVM.RoleList =  GetRoles();
            TempData["Error"] = "Role Name Is mandatory";
            return View(userVM);
        }
        userVM.UserReg.Isdeleted = false;

        await tryRegisterSaveImageAsync(userVM, fileProfileImage);
        await tryRegisterDeleteImageAsync(userVM, fileProfileImage);


        if (userVM.UserReg.Id == 0)
        {
            _db.Registrations.Add(userVM.UserReg);
        }
        else
        {
            _db.Registrations.Update(userVM.UserReg);
        }
        _db.SaveChanges();
        TempData["success"] = "Data saved Successfully";
        return RedirectToAction();
    }


    #region----FETCH AND DELETE IMAGE-----
    private async Task tryRegisterSaveImageAsync(UserRegisterVM userVM, IFormFile fileProfileImage)
    {
        if (fileProfileImage != null)
        {
            var uploadimg = $"{_webHostEnvironment.WebRootPath}{imageService.ProfileImagePath}";
            var fileName = await imageService.SaveImageAsync(fileProfileImage, uploadimg);
            userVM.UserReg.ImageUpload = $"{imageService.ProfileImagePath}{fileName}";
        }
    }





    private async Task tryRegisterDeleteImageAsync(UserRegisterVM userVM, IFormFile fileProfileImage)
    {
        var objfromdb = await _db.Registrations.Where(u => u.Isdeleted == false && u.Id == userVM.UserReg.Id).FirstOrDefaultAsync();
        if (fileProfileImage == null)
        {
            userVM.UserReg.ImageUpload = objfromdb?.ImageUpload;
        }
        else
        {
            imageService.DeleteImage(_webHostEnvironment.WebRootPath, objfromdb?.ImageUpload);
        }
    }
    #endregion

    #endregion

    #region---Fetch Method(APIs)---
    //[HttpPost]
    //public IActionResult Fetchmethod()
    //{
    //    var dataList = _db.Registrations.Where(u => u.Isdeleted == false).ToList();
    //    return Json(new { data = dataList });
    //}


    // Replace the problematic fetchmethod with the correct async/await usage and fix the variable declaration

    [HttpPost]
    public async Task<IActionResult> fetchmethod()
    {
        var registrations = await _db.Registrations.AsNoTracking()
            .Where(a => a.Isdeleted == false)
            .Include(a => a.RoleNavigation)
            .ToListAsync();

        var result = registrations.Select(u => new RegistrationDTOs
        {
            Id = u.Id,
            //RoleId = u.RoleId,
            Name = u.Name,

            Email = u.Email,
            Password = u.Password,
            //RoleName = u.RoleNavigation != null ? u.RoleNavigation.Name : string.Empty,
            Role = u.RoleNavigation!.Name,
            ImageUpload = u.ImageUpload
        }).ToList();

        // DataTables expects { data: [...] }
        return Json(new { data = result });
    }
    #endregion

    #region---------Advance Search Button------
    [HttpPost]
    public async Task<IActionResult> RegistrationAdvSearch(string? name, string? email, int? roleid, DateOnly? fromregisterDate, DateOnly? ToregisterDate)
    {
        var result = _db.Registrations.Where(s => s.Isdeleted == false
        && ((name != null) ? s.Name == name : true)
        && ((email != null) ? s.Email == email : true)
        && ((roleid > 0) ? s.RoleId == roleid : true)
        && ((fromregisterDate != null && ToregisterDate != null) ? s.RegistrationDate >= fromregisterDate && s.RegistrationDate <= ToregisterDate : true)
        ).Select(s => new RegistrationDTOs
        {
            Id = s.Id,
            Role = s.RoleNavigation!.Name,
            Name = s.Name,
            Email = s.Email,
            Password = s.Password,
            ImageUpload = s.ImageUpload,
            RegistrationDate = s.RegistrationDate
        }).ToList();

        // DataTables expects { data: [...] }
        return Json(new { data = result });
    }
    #endregion

    #region--Delete Method---
    [HttpDelete]
    public IActionResult DeleteHandler(int id)
    {
        var rode = _db.Registrations.FirstOrDefault(u => u.Isdeleted == false && u.Id == id);
        if (rode == null)
        {
            return Json(new { success = false, message = "Not Found Data" });
        }
        rode.Isdeleted = true;
        _db.Registrations.Update(rode);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Delete Successfully" });
    }
    #endregion

    #region---DownLoad Excel Format -----
    [HttpPost]
    public async Task<IActionResult> UserDetailsDownload()
    {
        var user = await _db.Registrations.AsNoTracking().Where(s => s.Isdeleted == false).Include(s => s.RoleNavigation).ToListAsync();
        var result = user.Select(s => new RegistrationDTOs
        {
            Id = s.Id,
            Role = s.RoleNavigation!.Name,
            Name = s.Name,
            Email = s.Email,
            Password = s.Password,
            ImageUpload = s.ImageUpload,
            RegistrationDate=s.RegistrationDate
        }).ToList();

        var returnobj = new
        {
            downloadAllow = 1,   // ✅ your sir's way
            data = new { result }
        };

        return new JsonResult(returnobj);
    }
    #endregion

    #endregion

    #region--Student List--
    public IActionResult Student()
    {
        return View();
    }

    #region---Fetch Method(APIs)---
    //[HttpPost]
    //public IActionResult Fetchmethod()
    //{
    //    var dataList = _db.Registrations.Where(u => u.Isdeleted == false).ToList();
    //    return Json(new { data = dataList });
    //}

    // Replace the problematic fetchmethod with the correct async/await usage and fix the variable declaration

    [HttpPost]
    public async Task<IActionResult> studentfetchmethod()
    {
        var registrations = await _db.Registrations.AsNoTracking()
            .Where(a => a.Isdeleted == false && a.RoleId == 2)
            .Include(a => a.RoleNavigation)
            .ToListAsync();

        var result = registrations.Select(u => new RegistrationDTOs
        {
            Id = u.Id,
            //RoleId = u.RoleId,
            Name = u.Name,

            Email = u.Email,
            Password = u.Password,
            //RoleName = u.RoleNavigation != null ? u.RoleNavigation.Name : string.Empty,
            Role = u.RoleNavigation!.Name,
            ImageUpload = u.ImageUpload
        }).ToList();

        // DataTables expects { data: [...] }
        return Json(new { data = result });
    }





    #endregion

    #region--Delete Method---
    [HttpDelete]
    public IActionResult StudentDeleteHandler(int id)
    {
        var rode = _db.Registrations.FirstOrDefault(u => u.Isdeleted == false && u.Id == id);
        if (rode == null)
        {
            return Json(new { success = false, message = "Not Found Data" });
        }
        rode.Isdeleted = true;
        _db.Registrations.Update(rode);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Delete Successfully" });
    }
    #endregion

    #region-------Student List Download Excel Format-----
    [HttpPost]
    public async Task<IActionResult> studentDownloadExcel()
    {
        var studentlist = await _db.Registrations.AsNoTracking().Where(s => s.Isdeleted == false && s.RoleId == 2).Include(s => s.RoleNavigation).ToListAsync();
        var result = studentlist.Select(s => new RegistrationDTOs
        {
            Id = s.Id,
            Role = s.RoleNavigation!.Name,
            Name = s.Name,
            Email = s.Email,
            Password = s.Password,
            ImageUpload = s.ImageUpload
        }).ToList();

        var studentOBJ = new
        {
            downloadAllow = 1,
            data = new { result }
        };
        return new JsonResult(studentOBJ);
    }
    #endregion


    #endregion


    #region--Teacher List--

    #region--GET Method--
    public IActionResult Teacher()
    {
        return View();
    }


    #endregion

    #region---Fetch Method(APIs)---


    // Replace the problematic fetchmethod with the correct async/await usage and fix the variable declaration
    [HttpPost]
    public async Task<IActionResult> Teacherfetchmethod()
    {
        var registrations = await _db.Registrations.AsNoTracking()
            .Where(a => a.Isdeleted == false && a.RoleId == 3)
            .Include(a => a.RoleNavigation)
            .ToListAsync();

        var result = registrations.Select(u => new RegistrationDTOs
        {
            Id = u.Id,
            //RoleId = u.RoleId,
            Name = u.Name,
            Email = u.Email,
            Password = u.Password,
            //RoleName = u.RoleNavigation != null ? u.RoleNavigation.Name : string.Empty,
            Role = u.RoleNavigation!.Name,
            ImageUpload = u.ImageUpload
        }).ToList();

        // DataTables expects { data: [...] }
        return Json(new { data = result });
    }





    #endregion

    #region--Delete Method---
    [HttpDelete]
    public IActionResult TeacherDeleteHandler(int id)
    {
        var rode = _db.Registrations.FirstOrDefault(u => u.Isdeleted == false && u.Id == id);
        if (rode == null)
        {
            return Json(new { success = false, message = "Not Found Data" });
        }
        rode.Isdeleted = true;
        _db.Registrations.Update(rode);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Delete Successfully" });
    }
    #endregion


    #region-----Download Excel Format----
    [HttpPost]
    public async Task<IActionResult> TeacherDownloadexcelFormat()
    {
        var TeacherLists = await _db.Registrations.AsNoTracking().Where(s => s.Isdeleted == false && s.RoleId == 3)
            .Include(s => s.RoleNavigation).ToListAsync();
        var result = TeacherLists.Select(s => new RegistrationDTOs
        {
            Id = s.Id,
            Role = s.RoleNavigation!.Name,
            Name = s.Name,
            Email = s.Email,
            Password = s.Password,
            ImageUpload = s.ImageUpload
        }).ToList();

        var teacherobj = new
        {
            downloadAllow = 1,
            data = new { result }

        };
        return new JsonResult(teacherobj);

    }
    #endregion




    #endregion


    #region----Subject----

    #region-----Get Method----
    public IActionResult Subject(int? id)
    {
        var subjectvm = new SubjectVM();
        if (id.HasValue && id != 0)
        {
            var role = _db.Subjects.Where(s => s.Id == id && s.Isdeleted == false).FirstOrDefault();
            if (role == null)
            {
                TempData["error"] = "No Data Found";
                return RedirectToAction();
            }
            subjectvm.subReg = role;
        }
        return View(subjectvm);
    }
    #endregion

    #region----Subject Post Method--------------
    [HttpPost]
    public IActionResult Subject(SubjectVM subjectvm)
    {
        
        subjectvm.subReg.Isdeleted = false;
        if (subjectvm.subReg.Id == 0)
        {
            _db.Subjects.Add(subjectvm.subReg);

        }
        else
        {
            _db.Subjects.Update(subjectvm.subReg);

        }
        _db.SaveChanges();
        TempData["success"] = "Data Saved Successfully";

        return RedirectToAction();

    }
    #endregion

    #region---Fetch Method---
    [HttpPost]
    public IActionResult subjectFetchMethod()
    {
        var dataList = _db.Subjects.Where(s => s.Isdeleted == false).ToList();
        return Json(new { data = dataList });
    }
    #endregion

    #region---Delete Method----
    [HttpDelete]
    public IActionResult SubjectDelete(int id)
    {
        var role = _db.Subjects.FirstOrDefault(s => s.Isdeleted == false && s.Id == id);
        if (role == null)
        {
            return Json(new { success = false, message = "Data Not Found" });
        }
        role.Isdeleted = true;
        _db.Subjects.Update(role);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Delete Successfully" });
    }
    #endregion

    #endregion


    #region----Result Names Registration---

    #region--Get Method--------
    public IActionResult Result(int? id)
    {
        var resultvm = new ResultVM();
        if (id.HasValue && id != 0)
        {
            var role = _db.Results.Where(r => r.Isdeleted == false && r.Id == id).FirstOrDefault();
            if (role == null)
            {
                TempData["error"] = "Data Not Found";
                return RedirectToAction();
            }
            resultvm.resReg = role;

        }

        return View(resultvm);
    }
    #endregion


    #region---Post Method-----
    [HttpPost]
    public IActionResult Result(ResultVM resultvm)
    {
        
        resultvm.resReg.Isdeleted = false;
        if (resultvm.resReg.Id == 0)
        {
            _db.Results.Add(resultvm.resReg);
        }
        else
        {
            _db.Results.Update(resultvm.resReg);
        }
        _db.SaveChanges();
        TempData["success"] = "Data Saved";

        return RedirectToAction();
    }
    #endregion

    #region---Fetch Method----
    public IActionResult ResultFetchMethod()
    {
        var dataList = _db.Results.Where(r => r.Isdeleted == false).ToList();
        return Json(new { data = dataList });

    }

    #endregion


    #region--Result Delete Method---
    public IActionResult resultDeleteHandler(int id)
    {
        var rode = _db.Results.FirstOrDefault(r => r.Isdeleted == false && r.Id == id);
        if (rode == null)
        {
            return Json(new { success = false, message = "Data Not Found" });
        }
        rode.Isdeleted = true;
        _db.Results.Update(rode);
        _db.SaveChanges();
        return Json(new { success = true, message = "Data Delete Successfully" });
    }
    #endregion



    #endregion


    #region---Student Result Edition---

    #region---Get Method----
    public async Task<IActionResult> ResultEdition(int? id)
    {
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

    #region---Post Method---
    [HttpPost]
    public IActionResult ResultEdition(StudentDetailsVM studentvm)
    {
        studentvm.studentDetailsReg.Isdeleted = false;
        if (studentvm.studentDetailsReg.Id != 0)
        {
            _db.StudentDetails.Update(studentvm.studentDetailsReg);
        }
        _db.SaveChanges();
        TempData["success"] = "Data Saved Successfully";
        return RedirectToAction();
    }
    #endregion

    #endregion


    #region--Pass Student fecth Details--

    #region--Get Method----
    public async Task<IActionResult> PassStudentList(int? id)
    {
        var studentdetailsvm = new StudentDetailsVM
        {
            NameList = await Getregister(),
            SubjectList = await GetSubject(),
            ResultList = await GetResult()
        };
        if (id.HasValue && id != 0)
        {
            var role = _db.StudentDetails.Where(s => s.Isdeleted == false && s.ResultId == 1).FirstOrDefault();
            if (role == null)
            {
                TempData["error"] = "Data Not Found";
                return RedirectToAction();
            }
            studentdetailsvm.studentDetailsReg = role;
        }

        return View(studentdetailsvm);
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
    public async Task<IActionResult> PassStudentListFetchmethod()
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

    #region----Advance Search Button----
    [HttpPost]
    public IActionResult PassStudentAdvanceSearch(int? studentnameId, string? studentclass, string? StudentSection, int? frommarks, int? toMarks)
    {
        var passstudent = _db.StudentDetails.
            Where(s => s.Isdeleted == false && s.ResultId == 1
         && ((studentnameId > 0) ? s.StudentId == studentnameId : true)
         && ((studentclass != null) ? s.Class == studentclass : true)
         && ((StudentSection != null) ? s.Section == StudentSection : true)
         && ((frommarks != 0 && toMarks != 0) ? (Convert.ToInt32(s.Marks) >= frommarks && Convert.ToInt32(s.Marks) <= toMarks) : true)).Select(s => new StudentDetailsDTOs
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
    public IActionResult PassDeleteHandler(int id)
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

    #region---Execel DownLoad format----
    [HttpPost]
    public async Task<IActionResult> PassStudentDownLoadExcel()
    {
        var passStudent = await _db.StudentDetails.AsNoTracking()
            .Where(s => s.Isdeleted == false && s.ResultId == 1).Include(s => s.Registration)
            .Include(s => s.Result).Include(s => s.Subjects).ToListAsync();
        var result = passStudent.Select(s => new StudentDetailsDTOs
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
        var passobj = new
        {
            downloadAllow = 1,
            data = new { result }
        };
        return new JsonResult(passobj);
    }
    #endregion

    #endregion

    #region----Fail Student Details----
    #region---Get Method----
    public async Task<IActionResult> FailStudentList(int? id)
    {
        var studentvm = new StudentDetailsVM()
        {
            NameList = await Getregister()
        };

        return View(studentvm);
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
    public async Task<IActionResult> FailFetchMethod()
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

    #region---Advance Search Form---
    [HttpPost]
    public IActionResult FailStudentAdvnaceForm(int? studentnameid, string? studentclass, string? studentsection)
    {
        var failStudent = _db.StudentDetails.Where(s => s.Isdeleted == false && s.ResultId == 2
        && ((studentnameid > 0) ? s.StudentId == studentnameid : true)
        && ((studentclass != null) ? s.Class == studentclass : true)
        && ((studentsection != null) ? s.Section == studentsection : true)).Select(s => new StudentDetailsDTOs
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

        return Json(new { data = failStudent });
    }
    #endregion

    #region--Delete Method---
    [HttpDelete]
    public IActionResult FailDeleteMethod(int id)
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

    #region---Download Excel Format-----
    [HttpPost]
    public async Task<IActionResult> FailStudentDownloadExecl()
    {
        var failstudents = await _db.StudentDetails.AsNoTracking()
            .Where(s => s.Isdeleted == false && s.ResultId == 2).Include(s => s.Registration)
            .Include(s => s.Result).Include(s => s.Subjects).ToListAsync();
        var result = failstudents.Select(s => new StudentDetailsDTOs
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
        var studentObj = new
        {
            downloadAllow = 1,
            data = new { result }

        };
        return new JsonResult(studentObj);
    }
    #endregion




    #endregion

}
