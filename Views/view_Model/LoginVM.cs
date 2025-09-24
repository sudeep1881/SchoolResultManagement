using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.Views.view_Model;

public class LoginVM
{
   

    [Required]
    [EmailAddress]
    [MaxLength(300)]
    [DisplayName("Email address")]
    public string EmailAddress { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
