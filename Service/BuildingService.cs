using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using SIS.Data;
using SIS.Models;
namespace WebApi.Services 
{ 
    public interface IBuildingService    
    {         
        Building Create(Building model);
        Building Update(Building model);
        string Delete(Building model);
        IEnumerable<Building> GetAll();
        Building GetBuildingInfo(int id); 
    }
    public class BuildingService: IBuildingService     
    { 
        private ApplicationDbContext _context; 
        private readonly AppSettings _appSettings; 
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        RoleManager<IdentityRole> _roleManager;
    public BuildingService(ApplicationDbContext context, UserManager < IdentityUser > userManager, SignInManager < IdentityUser > signInManager,
    ILogger < RegisterModel > logger, IEmailSender emailSender, RoleManager < IdentityRole > roleManager, IOptions < AppSettings > appSettings)         
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
            _appSettings = appSettings.Value; 
        }
    public Building Create(Building model) 
        {
            Guid gid = Guid.NewGuid();
            _context.Add(model);
            _context.SaveChanges();
            return model; 
        }
    public Building Update(Building model) 
        {
            _context.Update(model);
            _context.SaveChanges();
            return model; 
        }
    public Building GetBuildingInfo(int id) 
        {
            var model = _context.Buildings.FirstOrDefault(m => m.BuildingID == id);
            var URLImage = _appSettings.URLImage; return model; 
        }
    public IEnumerable<Building> GetAll() 
        {
            var URLImage = _appSettings.URLImage;
            var model = (from c in _context.Buildings select new Building
                            { 
                                BuildingID = c.BuildingID,
                                BuildingCode = c.BuildingCode,
                                BuildingName = c.BuildingName,
                                NumberofFloors = c.NumberofFloors,
                            }).ToList(); 
            return model;
        } 
    public string Delete(Building model)
    {
        string msg = "DELETED";
            try
            {
                _context.Buildings.Remove(model);
                _context.SaveChanges();
            }
            catch (Exception e) { msg = e.Message; 
            }
            return msg;
        }
    } 
}