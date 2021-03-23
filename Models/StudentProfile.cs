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
    public class StudentProfile
    {
        [DisplayName("Student ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Column(TypeName = "char(1)")]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

        [Column(TypeName = "char(11)")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "varchar(100)")]
        [DisplayName("Student Picture")]
        public string Picture { get; set; }

        [DisplayName("Province Name")]
        public int ProvinceID { get; set; }

        [DisplayName("City Name")]
        public int CityID { get; set; }

        [DisplayName("Program Name")]
        public int ProgramID { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public string PictureImageUrl { get; set; }

        [Column(TypeName = "char(36)")]
        [DisplayName("FileStamp")]
        public string FileStamp { get; set; }

        [NotMapped]
        public IFormFile UploadFiles { set; get; }

        [NotMapped]
        public ICollection<SelectListItem> IProvince { get; set; }

        [NotMapped]
        public ICollection<SelectListItem> ICity { get; set; }        

        [NotMapped]
        public ICollection<SelectListItem> IProgram { get; set; }        
    }
}
