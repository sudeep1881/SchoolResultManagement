using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAttendanceManager.Helpers;
using SchoolAttendanceManager.Image_Services;
using SchoolAttendanceManager.Infrastructure.Email;
using SchoolAttendanceManager.Models;
using SchoolAttendanceManager.Views.view_Model;
using System.Security.Claims;
namespace SchoolAttendanceManager.Controllers;

public class LoginController : Controller
{

    private readonly SchoolAttendanceDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IEmailSender _email;
    private readonly IConfiguration _configuration;

    public LoginController(SchoolAttendanceDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender email, IConfiguration configuration)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
        _email = email;
        _configuration = configuration;
    }

    #region--Login--

    #region--GET METHOD--

    public IActionResult Index()
    {

        var loginVM = new LoginVM();

        return View(loginVM);
    }

    #endregion

    #region--Post Method---
    [HttpPost]
    public async Task<IActionResult> Index(LoginVM logimvm)
    {

        var user = await _db.Registrations
            .Where(s => s.Isdeleted == false &&
                s.Email!.Trim().ToLower() == logimvm.EmailAddress.Trim().ToLower() &&
                s.Password == logimvm.Password)
            .Select(s => new Registration
            {
                Id = s.Id,
                Name = s.Name,
                RoleId = s.RoleId,
                Email = s.Email,
                Role = s.RoleNavigation!.Name
            })
            .FirstOrDefaultAsync();
            
        if (user == null)
        {
            TempData["error"] = "Invalid Email or Password";
            return View(logimvm);
        }

        // ===============================
        // 🔐 ADD THIS BLOCK (COOKIE LOGIN)
        // ===============================

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.Role, user.Role!) // must match "Admin"
    };   

        var identity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        // ===============================>
        // 🔐 JWT (optional, keep for API)
        var jwtHelper = new JwtTokenHelper(_configuration);
        var token = jwtHelper.GenerateToken(user.Id, user.Email!, user.Role!);

        HttpContext.Session.SetString("JWToken", token);

        HttpContext.Session.SetInt32(SD.KeyUser, user.Id);
        HttpContext.Session.SetInt32(SD.KeyRole, (int)user.RoleId!);

        // Role-based redirect
        return user.RoleId switch
        {
            1 => RedirectToAction(nameof(AdminController.Dashboard), "Admin"),
            2 => RedirectToAction(nameof(StudentController.StudentList), "Student"),
            3 => RedirectToAction(nameof(TeacherController.Dashboard), "Teacher"),
            _ => RedirectToAction("Index")
        };
    }

    #endregion

    #endregion

    //#region---LogOut Method----
    //public async Task<IActionResult> Logout()
    //{
    //    HttpContext.Session.Clear();

    //    // Fix: Use the correct controller and action names as strings
    //    return RedirectToAction("Login", "Usre", new { area = "Adminstation" });
    //}

    //#endregion


    #region-- Student Register-----

    #region--Get Method-----
    public IActionResult StudentRegister(int? id)
    {
        var registervm = new UserRegisterVM();
        if (id.HasValue && id != 0)
        {
            var role = _db.Registrations.Where(s => s.Isdeleted == false && s.Id == id).FirstOrDefault();
            if (role == null)
            {

                TempData["error"] = "Data Not Found";
                return RedirectToAction();
            }
            registervm.UserReg = role;
        }
        return View(registervm);
    }
    #endregion

    #region---Post Method---
    [HttpPost]
    public async Task<IActionResult> StudentRegister(UserRegisterVM userregistervm, IFormFile fileProfileImage)
    {
        userregistervm.UserReg.Isdeleted = false;

        await TrySaveImageProfileAsync(userregistervm, fileProfileImage);
        await TryDeleteImageProfileAsync(userregistervm, fileProfileImage);
        if (userregistervm.UserReg.Id == 0)
        {
            _db.Registrations.Update(userregistervm.UserReg);
        }
        _db.SaveChanges();
        TempData["success"] = "Data Save Succesfully";
        return RedirectToAction("Index");
    }




    #endregion

    #region---Save Image & Delete Image-----
    private async Task TrySaveImageProfileAsync(UserRegisterVM userregistervm, IFormFile fileProfileImage)
    {
        if (fileProfileImage != null)
        {
            var uploadImage = $"{_webHostEnvironment.WebRootPath}{imageService.ProfileImagePath}";
            var fileName = await imageService.SaveImageAsync(fileProfileImage, uploadImage);
            userregistervm.UserReg.ImageUpload = $"{imageService.ProfileImagePath}{fileName}";
        }
    }

    private async Task TryDeleteImageProfileAsync(UserRegisterVM userregistervm, IFormFile fileProfileImage)
    {
        var objfromdb = await _db.Registrations.Where(s => s.Isdeleted == false && s.Id == userregistervm.UserReg.Id).FirstOrDefaultAsync();
        if (fileProfileImage == null)
        {
            userregistervm.UserReg.ImageUpload = objfromdb?.ImageUpload;
        }
        else
        {
            imageService.DeleteImage(_webHostEnvironment.WebRootPath, objfromdb?.ImageUpload);
        }
    }
    
    #endregion
    

    #endregion


    #region-----Forget Password Method with Email Sender-----

    #region--Get Method -----
    public IActionResult ForgotPassword()
    {
        var model = new ForgotPasswordVM();
        return View(model);
    }
    #endregion


    #region--Post Method---
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _db.Registrations.FirstOrDefault(u => u.Email == model.Email && u.Isdeleted == false);

        if (user == null)
        {
            TempData["Error"] = "Email not found!";
            return View();
        }

        // Generate OTP
        var otp = new Random().Next(100000, 999999).ToString();

        

        // Store OTP in TempData (or DB if you want more secure)
        HttpContext.Session.SetString("OTP", otp);
        HttpContext.Session.SetString("Email", model.Email);

        try
        {
            // 1. Load HTML template
            // If your file is in Views/EmailTest/WelcomeEmail.cshtml
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "WelcomeEmail.cshtml");
            var htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);

            // 2. Get username from email
            string userName = model.Email.Split('@')[0];

            // 3. Replace placeholders
            htmlBody = htmlBody.Replace("{UserName}", userName);
            htmlBody = htmlBody.Replace("{Link}", "https://ResultCenter.com");
            htmlBody = htmlBody.Replace("{OTP}", otp);

            // 4. Send email
            await _email.SendAsync(model.Email, "Password Reset OTP", htmlBody);

            TempData["success"] = "Email sent successfully";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Email failed: {ex.Message}";
        }

        return RedirectToAction("VerifyOtp");
    }
    #endregion

    #endregion



    #region---Reset Password Method----

    #region---Get Method-----
    public IActionResult ResetPassword(string email)
    {
        var model = new ResetPasswordVM { Email = email };
        return View(model);
    }
    #endregion

    #region--Post Method----
    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _db.Registrations.FirstOrDefault(u => u.Email == model.Email && u.Isdeleted == false);

        if (user == null)
        {
            TempData["Error"] = "User not found!";
            return View(model);
        }

        // Update password in DB
        user.Password = model.NewPassword;  // ⚠️ better to hash password in real projects
        _db.Registrations.Update(user);
        _db.SaveChanges();

        TempData["Success"] = "Password reset successfully. Please login.";
        return RedirectToAction("Index", "Login");
    }
    #endregion

    #endregion

    #region OTP Verify

    #region--Get Method----
    public IActionResult VerifyOtp()
    {
        return View();
    }
    #endregion


    #region--Post Method-----
    [HttpPost]
    public IActionResult VerifyOtp(string otp)
    {
        // Get stored OTP and Email from Session
        var storedOtp = HttpContext.Session.GetString("OTP");
        var email = HttpContext.Session.GetString("Email");

        if (storedOtp == null || email == null)
        {
            TempData["Error"] = "Session expired. Please request a new OTP.";
            return RedirectToAction("ForgetPassword");
        }

        if (storedOtp == otp)
        {
            // Clear OTP after successful verification (optional but recommended)
            HttpContext.Session.Remove("OTP");

            return RedirectToAction("ResetPassword", new { email });
        }

        TempData["Error"] = "Invalid OTP!";
        return View();
    }
    #endregion
    #endregion

}
