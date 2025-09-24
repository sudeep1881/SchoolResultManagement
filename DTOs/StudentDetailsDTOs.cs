using SchoolAttendanceManager.Models;

namespace SchoolAttendanceManager.DTOs
{
    public class StudentDetailsDTOs
    {
        public int IdDTO { get; set; }

        //public int? StudentIdDTO { get; set; }
        public string? StudentNameDTO { get; set; }

        public string? ClassDTO { get; set; }

        public string? SectionDTO { get; set; }

        //public int? SubjectIdDTO { get; set; }
        public string? SubjectNameDTO { get; set; }

        public string? MarksDTO { get; set; }

        public string? PercentageDTO { get; set; }

        //public int? ResultIdDTO { get; set; }
        public string? ResultNameDTO { get; set; }



    }
}
