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
    public class Semester
    {

        [DisplayName("AY Sem ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AYSemID { get; set; }

        [Column(TypeName = "char(9)")]
        [DisplayName("Academic Year")]
        public string AcademicYear { get; set; }

        [Column(TypeName = "char(15)")]
        [DisplayName("Semester Name")]
        public string SemesterName { get; set; }
    }
}
