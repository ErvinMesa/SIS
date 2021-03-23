using SIS.Models;
using System.Text.Json.Serialization;
using WebApi.Entities;

namespace WebApi.Models
{
    public class AuthenticateResponse
    {
        
        public int Id { get; set; }

        public string Username { get; set; }

        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(UserToken user, string jwtToken, string refreshToken)
        {

            Id = user.Id;
            Username = user.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;

        }
    }
}