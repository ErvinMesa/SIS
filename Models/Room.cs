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
    public class Room
    {

        [DisplayName("Room ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomID { get; set; }

        [DisplayName("Building Name")]
        public int BuildingID { get; set; }

        [Column(TypeName = "char(10)")]
        [DisplayName("Room Code")]
        public string RoomCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        [DisplayName("Room Name")]
        public string RoomName { get; set; }

        [DisplayName("Capacity")]
        public int Capacity { get; set; }


        
        [NotMapped]
        public ICollection<SelectListItem> IBuilding { get; set; }

    }
}
