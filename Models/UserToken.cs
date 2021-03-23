using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebApi.Entities;

namespace SIS.Models
{

    public class UserToken

    {

        [DisplayName("TokenID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}