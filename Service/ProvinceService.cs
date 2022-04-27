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
    public interface IProvinceService
    {
        Province Create(Province model);
        Province Update(Province model);
        string Delete(Province model);
        IEnumerable<Province> GetAll();
        Province GetProvinceInfo(int id);
    }
    public class ProvinceService : IProvinceService
    {
        private ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;


        public ProvinceService(
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

        // public Province Create(Province model)
        // {
        //     _context.Add(model);
        //     _context.SaveChanges();
        //     return model;
        // }
        public Province Create(Province model)
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Province Update(Province model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }

        public Province GetProvinceInfo(int id)
        {
            var model = _context.Provinces.FirstOrDefault(m => m.ProvinceID == id);
            return model;
        }

        public IEnumerable<Province> GetAll()
        {
            var model = (from c in _context.Provinces
                         select new Province
                         {
                             ProvinceID = c.ProvinceID,
                             ProvinceName = c.ProvinceName,
                         }).ToList();

            return model;
        }

        public string Delete(Province model)
        {
            string msg = "DELETED";
            try
            {
                _context.Provinces.Remove(model);
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