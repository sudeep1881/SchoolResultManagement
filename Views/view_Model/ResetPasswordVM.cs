using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Views.view_Model
{
    public class ResetPasswordVM
    {
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage =" Password Is Mandaroty")]


        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage ="Confirm Password is requied")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password is dont not Match")]
        public string ConfirmedPassword { get; set; }
    }
}
