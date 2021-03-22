using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SIS.Models
{
    public class CourseSchedule
    {

        [DisplayName("Schedule Course ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleCourseID { get; set; }

        [DisplayName("Course Name")]
        public int CourseID { get; set; }

        [DisplayName("Semester Name")]
        public int AYSemID { get; set; }

        [DisplayName("Faculty Name")]
        public int FacultyID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Days")]
        public string Days { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Time From")]
        public string TimeFrom { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("TimeTo")]
        public string TimeTo { get; set; }

        [DisplayName("RoomID")]
        public int RoomID { get; set; }
        
        [NotMapped]
        public ICollection<SelectListItem> ICourse { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> IRoom { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> ISemester { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> IFaculty { get; set; }

    }
}
