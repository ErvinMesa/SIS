using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Entities;
using WebApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using SIS.Data;
using SIS.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<UserToken> GetAll();
        UserToken GetById(int id);
    }

    public class UserService : IUserService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public UserService(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {
            //var user =  _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            var result = _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
            UserToken user = new UserToken();
            // return null if user not found
            if (result == null)
            {
                return null;
            }
   
            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);


            user = (from u in _context.UserTokens where u.Username == model.Username select u).First();
            // save refresh token

            if (user == null)
            {

                user.Username = model.Username;
                user.RefreshTokens = new List<RefreshToken>();
                user.RefreshTokens.Add(refreshToken);
                _context.Add(user);
                _context.SaveChanges();
            }
            else
            {
                var newRefreshToken = generateRefreshToken(ipAddress);
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReplacedByToken = newRefreshToken.Token;

                user.Username = model.Username;
                user.RefreshTokens = new List<RefreshToken>();
                user.RefreshTokens.Add(newRefreshToken);
                _context.Update(user);
                _context.SaveChanges();
            }

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _context.UserTokens.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public bool RevokeToken(string token, string ipAddress)
        {
            var user = _context.UserTokens.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        public IEnumerable<UserToken> GetAll()
        {
            return _context.UserTokens;
        }

        public UserToken GetById(int id)
        {
            return _context.UserTokens.Find(id);
        }

        // helper methods

        private string generateJwtToken(UserToken user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}