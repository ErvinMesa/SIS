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
    public class Building
    {
        [DisplayName("Building ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BuildingID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Building Code")]
        public string BuildingCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Building Name")]
        public string BuildingName { get; set; }

        [DisplayName("Number of Floors")]
        public int NumberofFloors { get; set; }
    }
}
