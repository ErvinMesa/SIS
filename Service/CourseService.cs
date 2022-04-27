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
    public interface ICourseService
    {
        Course Create(Course model);
        Course Update(Course model);
        string Delete(Course model);
        IEnumerable<Course> GetAll();
        Course GetCourseInfo(int id);
    }
    public class CourseService : ICourseService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public CourseService(
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

        // public Course Create(Course model)
        // {
        //     _context.Add(model);
        //     _context.SaveChanges();
        //     return model;
        // }
        public Course Create(Course model)
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Course Update(Course model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public Course GetCourseInfo(int id)
        {
            var model = _context.Courses.FirstOrDefault(m => m.CourseID == id);
            return model;
        }

        public IEnumerable<Course> GetAll()
        {
            var model = (from c in _context.Courses
                         select new Course
                         {
                             CourseID = c.CourseID,
                             CourseCode = c.CourseCode,
                             CourseDescription = c.CourseDescription,
                             AcademicUnit = c.AcademicUnit,
                             ContactHours = c.ContactHours,
                             TuitionUnit = c.TuitionUnit,
                         }).ToList();

            return model;
        }

        public string Delete(Course model)
        {
            string msg = "DELETED";
            try
            {
                _context.Courses.Remove(model);
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