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
    public class City
    {
        [DisplayName("City ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityID { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("City Name")]
        public string CityName { get; set; }
    }
}
