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
    public interface ISemesterService
    {
        Semester Create(Semester model);
        Semester Update(Semester model);
        string Delete(Semester model);
        IEnumerable<Semester> GetAll();
        Semester GetSemesterInfo(int id);
    }
    public class SemesterService : ISemesterService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public SemesterService(
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

        // public Semester Create(Semester model)
        // {
        //     _context.Add(model);
        //     _context.SaveChanges();
        //     return model;
        // }
        public Semester Create(Semester model)
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Semester Update(Semester model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public Semester GetSemesterInfo(int id)
        {
            var model = _context.Semesters.FirstOrDefault(m => m.AYSemID == id);
            return model;
        }

        public IEnumerable<Semester> GetAll()
        {
            var model = (from c in _context.Semesters
                         select new Semester
                         {
                             AYSemID = c.AYSemID,
                             AcademicYear = c.AcademicYear,
                             SemesterName = c.SemesterName,
                         }).ToList();

            return model;
        }

        public string Delete(Semester model)
        {
            string msg = "DELETED";
            try
            {
                _context.Semesters.Remove(model);
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