using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Views.view_Model
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email  is Mandatory")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string? Email { get; set; }
    }
}
