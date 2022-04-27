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
    public interface IFacultyService
    {
        Faculty Create(Faculty model);
        Faculty Update(Faculty model);
        string Delete(Faculty model);
        IEnumerable<Faculty> GetAll();
        Faculty GetFacultyInfo(int id);
    }
    public class FacultyService : IFacultyService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public FacultyService(
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

        // public Faculty Create(Faculty model)
        // {
        //     _context.Add(model);
        //     _context.SaveChanges();
        //     return model;
        // }
        public Faculty Create(Faculty model)
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Faculty Update(Faculty model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public Faculty GetFacultyInfo(int id)
        {
            var model = _context.Faculties.FirstOrDefault(m => m.FacultyID == id);
            return model;
        }

        public IEnumerable<Faculty> GetAll()
        {
            var model = (from c in _context.Faculties
                         select new Faculty
                         {
                             FacultyID = c.FacultyID,
                             FacultyName = c.FacultyName,
                         }).ToList();

            return model;
        }

        public string Delete(Faculty model)
        {
            string msg = "DELETED";
            try
            {
                _context.Faculties.Remove(model);
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