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
    public class EnrolledCourse
    {

        [DisplayName("Record ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordID { get; set; }

        [DisplayName("Student Name")]
        public string StudentID { get; set; }

        [DisplayName("Schedule Course ID")]
        public string ScheduleCourseID { get; set; }
        
        [NotMapped]
        public ICollection<SelectListItem> IStudent { get; set; }
        [NotMapped]
        public ICollection<SelectListItem> ICourseSchedule { get; set; }
    }
}
