using Microsoft.AspNetCore.Http;
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
    public class College
    {
        [DisplayName("College ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollegeID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("College Code")]
        public string CollegeCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("College Name")]
        public string CollegeName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Dean")]
        public string NameofDean { get; set; }

        public bool IsActive { get; set; }

        [DisplayName("Date Recognize")]
        public DateTime RecognizeDate { get; set; }

        [DisplayName("No. of Prog.")]
        public int NumberOfProgram { get; set; }

        [Column(TypeName = "varchar(100)")]
        [DisplayName("College Logo")]
        public string Logo { get; set; }

        [NotMapped]
        public string LogoImageUrl { get; set; }

        [Column(TypeName = "char(36)")]
        [DisplayName("FileStamp")]
        public string FileStamp { get; set; }

        [NotMapped]
        public IFormFile UploadFiles { set; get; }
    }
}