using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using SIS.Data;
using SIS.Models;

namespace SIS.Controllers
{
    public class StudentProfileController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public StudentProfileController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();        
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all ModeOfPayments data
                var objData = (from t in _context.StudentProfiles
                                join p in _context.Provinces on t.ProvinceID equals p.ProvinceID into pl
                                from w in pl.DefaultIfEmpty()
                                join c in _context.Cities on t.CityID equals c.CityID into cl
                                from x in cl.DefaultIfEmpty()
                                join y in _context.EnrolledPrograms on t.ProgramID equals y.ProgramID into ep
                                from z in ep.DefaultIfEmpty()
                               select new
                               {
                                   StudentID = t.StudentID,
                                   LastName = t.LastName,
                                   FirstName = t.FirstName,
                                   MiddleName = t.MiddleName,
                                   Gender = t.Gender,
                                   BirthDate = t.BirthDate,
                                   MobileNumber = t.MobileNumber,
                                   EmailAddress = t.EmailAddress,
                                   ProvinceID = t.ProvinceID,
                                   ProvinceName = w == null ? ("No ProvinceName") : w.ProvinceName,
                                   CityID = t.CityID,
                                   CityName = x == null ? ("No CityName") : x.CityName,
                                   ProgramID = t.ProgramID,
                                   ProgramName = z == null ? ("No ProgramName") : z.ProgramName,
                               });

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.LastName.Contains(searchValue));
                }

                //total number of rows count
                recordsTotal = objData.Count();
                //Paging
                var data = await objData.Skip(skip).Take(pageSize).ToListAsync();
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
public async Task<IActionResult> AddEditStudentProfile(int StudentID)
        {
            Guid gid = Guid.NewGuid();
            StudentProfile model = new StudentProfile();
            model.IProvince = _context.Provinces
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.ProvinceID.ToString(),
                                   Text = a.ProvinceName
                               }).OrderBy(a => a.Value)
                               .ToList();
            model.ICity = _context.Cities
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.CityID.ToString(),
                                   Text = a.CityName
                               }).OrderBy(a => a.Value)
                               .ToList();
            model.IProgram = _context.EnrolledPrograms
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.ProgramID.ToString(),
                                   Text = a.ProgramName
                               }).OrderBy(a => a.Value)
                               .ToList();
            string userID = User.Identity.Name;

            if (StudentID != 0)
            {
                model = await _context.StudentProfiles
                 .FirstOrDefaultAsync(m => m.StudentID == StudentID);
            }

            return PartialView("_AddEditStudentProfile", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditStudentProfile(StudentProfile model)
        {
            if (ModelState.IsValid)
            {
                if (model.StudentID == 0)
                {
                    _context.Add(model);
                }
                else
                {
                    _context.Update(model);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStudentProfile(int StudentProfileID)
        {
            string name = string.Empty;
            StudentProfile model = new StudentProfile();
            if (StudentProfileID != 0)
            {

                model = await _context.StudentProfiles
                 .FirstOrDefaultAsync(m => m.StudentID == StudentProfileID);
                if (model != null)
                {
                    name = model.LastName;
                }
            }
            return PartialView("_DeleteStudentProfile", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteStudentProfile(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["StudentProfileID"]);
            var objCons = await _context.StudentProfiles
                 .FirstOrDefaultAsync(m => m.StudentID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.LastName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.LastName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.LastName);
            }


            _context.StudentProfiles.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool StudentProfileExists(int id)
        {
            return _context.StudentProfiles.Any(e => e.StudentID == id);
        }
    }
}
