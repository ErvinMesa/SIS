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
    public class EnrolledProgram
    {

        [DisplayName("Program ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProgramID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Program Code")]
        public string ProgramCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Program Name")]
        public string ProgramName { get; set; }
        
        [DisplayName("College Name")]
        public int CollegeID { get; set; }

        [DisplayName("Number of Semester")]
        public int NumberofSemester { get; set; }


        
        [NotMapped]
        public ICollection<SelectListItem> ICollege { get; set; }

    }
}
