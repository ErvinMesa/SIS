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
    public class Faculty
    {

        [DisplayName("Faculty ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacultyID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Faculty Name")]
        public string FacultyName { get; set; }
    }
}
