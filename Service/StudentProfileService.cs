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
    public interface IStudentProfileService
    {
        StudentProfile Create(StudentProfile model);
        StudentProfile Update(StudentProfile model);
        string Delete(StudentProfile model);
        IEnumerable<StudentProfile> GetAll();
        StudentProfile GetStudentProfileInfo(int id);
    }
    public class StudentProfileService : IStudentProfileService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public StudentProfileService(
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

        // public StudentProfile Create(StudentProfile model)
        // {
            //     _context.Add(model);
            //     _context.SaveChanges();
            //     return model;
        // }
        public StudentProfile Create(StudentProfile model)
        {
            Guid gid = Guid.NewGuid();
            model.FileStamp = gid.ToString();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public StudentProfile Update(StudentProfile model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public StudentProfile GetStudentProfileInfo(int id)
        {
            var model = _context.StudentProfiles.FirstOrDefault(m => m.StudentID == id);
            var URLImage = _appSettings.URLImage;
            model.PictureImageUrl = URLImage + model.Picture;
            return model;
        }

        public IEnumerable<StudentProfile> GetAll()
        {
            var URLImage = _appSettings.URLImage;
            var model = (from c in _context.StudentProfiles
                        join p in _context.Provinces on c.ProvinceID equals p.ProvinceID into pl
                        from pd in pl.DefaultIfEmpty()
                        join ct in _context.Cities on c.CityID equals ct.CityID into cti
                        from ctd in cti.DefaultIfEmpty()
                        join ep in _context.EnrolledPrograms on c.ProgramID equals ep.ProgramID into epl
                        from epd in epl.DefaultIfEmpty()
                        select new StudentProfile
                        {
                            StudentID = c.StudentID,
                            LastName = c.LastName,
                            FirstName = c.FirstName,
                            MiddleName = c.MiddleName,
                            Gender = c.Gender,
                            BirthDate = c.BirthDate,
                            MobileNumber = c.MobileNumber,
                            EmailAddress = c.EmailAddress,
                            ProvinceID = c.ProvinceID,
                            CityID = c.CityID,
                            ProgramID = c.ProgramID,
                            IsActive = c.IsActive,
                            Picture = URLImage + c.Picture,
                            PictureImageUrl = c.PictureImageUrl,
                            FileStamp = c.FileStamp,
                        }).ToList();

            return model;
        }

        public string Delete(StudentProfile model)
        {
            string msg = "DELETED";
            try
            {
                _context.StudentProfiles.Remove(model);
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