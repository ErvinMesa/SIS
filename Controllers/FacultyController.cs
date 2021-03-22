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
    public class FacultyController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public FacultyController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
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
                var objData = (from t in _context.Faculties
                               select new
                               {
                                   FacultyID = t.FacultyID,
                                   FacultyName = t.FacultyName,
                               });

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.FacultyName.Contains(searchValue));
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
        public async Task<IActionResult> AddEditFaculty(int FacultyID)
        {
            Guid gid = Guid.NewGuid();
            Faculty model = new Faculty();
            string userID = User.Identity.Name;

            if (FacultyID != 0)
            {
                model = await _context.Faculties
                 .FirstOrDefaultAsync(m => m.FacultyID == FacultyID);
            }

            return PartialView("_AddEditFaculty", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditFaculty(Faculty model)
        {
            if (ModelState.IsValid)
            {
                if (model.FacultyID == 0)
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
        public async Task<IActionResult> DeleteFaculty(int FacultyID)
        {
            string name = string.Empty;
            Faculty model = new Faculty();
            if (FacultyID != 0)
            {

                model = await _context.Faculties
                 .FirstOrDefaultAsync(m => m.FacultyID == FacultyID);
                if (model != null)
                {
                    name = model.FacultyName;
                }
            }
            return PartialView("_DeleteFaculty", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteFaculty(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["FacultyID"]);
            var objCons = await _context.Faculties
                 .FirstOrDefaultAsync(m => m.FacultyID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.FacultyName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.FacultyName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.FacultyName);
            }


            _context.Faculties.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool FacultyExists(int id)
        {
            return _context.Faculties.Any(e => e.FacultyID == id);
        }
    }
}
