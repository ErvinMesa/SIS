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
    public interface ICollegeService
    {
        College Create(College model);
        College Update(College model);
        string Delete(College model);
        IEnumerable<College> GetAll();
        College GetCollegeInfo(int id);
    }

    public class CollegeService : ICollegeService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public CollegeService(
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

        public College Create(College model)
        {
            Guid gid = Guid.NewGuid();
            model.FileStamp = gid.ToString();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public College Update(College model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public College GetCollegeInfo(int id)
        {
            var model = _context.Colleges.FirstOrDefault(m => m.CollegeID == id);
            var URLImage = _appSettings.URLImage;
            model.LogoImageUrl = URLImage + model.Logo;
            return model;
        }

        public IEnumerable<College> GetAll()
        {
            var URLImage = _appSettings.URLImage;
            var model = (from c in _context.Colleges
                         select new College
                         {
                             CollegeID = c.CollegeID,
                             CollegeCode = c.CollegeCode,
                             CollegeName = c.CollegeName,
                             FileStamp = c.FileStamp,
                             IsActive = c.IsActive,
                             RecognizeDate = c.RecognizeDate,
                             NumberOfProgram = c.NumberOfProgram,
                             NameofDean = c.NameofDean,
                             Logo = URLImage + c.Logo,
                             LogoImageUrl = c.LogoImageUrl
                         }).ToList();

            return model;
        }

        public string Delete(College model)
        {
            string msg = "DELETED";
            try
            {
                _context.Colleges.Remove(model);
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