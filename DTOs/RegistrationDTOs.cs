using System.ComponentModel.DataAnnotations;

namespace SchoolAttendanceManager.DTOs;

public class RegistrationDTOs
{
    public int Id { get; set; }

 
    public string? Role { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }


    //public int? RoleId { get; set; }
    public string? ImageUpload { get; set; }

    public DateOnly? RegistrationDate { get; set; }

}
