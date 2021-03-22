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
    public class CollegeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public CollegeController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
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
                var objData = (from t in _context.Colleges
                               select new
                               {
                                   CollegeID = t.CollegeID,
                                   CollegeCode = t.CollegeCode,
                                   CollegeName = t.CollegeName,
                                   NameofDean = t.NameofDean
                               });

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.CollegeName.Contains(searchValue));
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
        public async Task<IActionResult> AddEditCollege(int CollegeID)
        {
            Guid gid = Guid.NewGuid();
            College model = new College();
            string userID = User.Identity.Name;

            if (CollegeID != 0)
            {
                model = await _context.Colleges
                 .FirstOrDefaultAsync(m => m.CollegeID == CollegeID);
            }

            return PartialView("_AddEditCollege", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditCollege(College model)
        {
            if (ModelState.IsValid)
            {
                if (model.CollegeID == 0)
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
        public async Task<IActionResult> DeleteCollege(int CollegeID)
        {
            string name = string.Empty;
            College model = new College();
            if (CollegeID != 0)
            {

                model = await _context.Colleges
                 .FirstOrDefaultAsync(m => m.CollegeID == CollegeID);
                if (model != null)
                {
                    name = model.CollegeName;
                }
            }
            return PartialView("_DeleteCollege", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCollege(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["CollegeID"]);
            var objCons = await _context.Colleges
                 .FirstOrDefaultAsync(m => m.CollegeID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.CollegeName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.CollegeName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.CollegeName);
            }


            _context.Colleges.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool CollegeExists(int id)
        {
            return _context.Colleges.Any(e => e.CollegeID == id);
        }
    }
}
