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
    public class Province
    {
        [DisplayName("Province ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProvinceID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }
    }
}
