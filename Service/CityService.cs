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
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface ICityService
    {
        City Create(City model);
        City Update(City model);
        string Delete(City model);
        IEnumerable<City> GetAll();
        City GetCityInfo(int id);
    }
    public class CityService : ICityService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public CityService(
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

        // public City Create(City model)
        // {
        //     _context.Add(model);
        //     _context.SaveChanges();
        //     return model;
        // }
        public City Create(City model)
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public City Update(City model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public City GetCityInfo(int id)
        {
            var model = _context.Cities.FirstOrDefault(m => m.CityID == id);
            return model;
        }

        public IEnumerable<City> GetAll()
        {
            var model = (from c in _context.Cities
                         select new City
                         {
                             CityID = c.CityID,
                             CityName = c.CityName,
                         }).ToList();

            return model;
        }

        public string Delete(City model)
        {
            string msg = "DELETED";
            try
            {
                _context.Cities.Remove(model);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                msg = e.Message;
            }
            return msg;

        }
    }
}