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
    public class Course
    {
        [DisplayName("Course ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Course Code")]
        public string CourseCode { get; set; }

        [Column(TypeName = "varchar(150)")]
        [DisplayName("Course Description")]
        public string CourseDescription { get; set; }

        [DisplayName("Academic Unit")]
        public double AcademicUnit { get; set; }

        [DisplayName("Contact Hours")]
        public double ContactHours { get; set; }

        [DisplayName("Tuition Unit")]
        public double TuitionUnit { get; set; }
    }
}
