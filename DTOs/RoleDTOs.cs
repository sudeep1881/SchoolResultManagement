using SchoolAttendanceManager.Models;

namespace SchoolAttendanceManager.DTOs
{
    public class RoleDTOs
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
