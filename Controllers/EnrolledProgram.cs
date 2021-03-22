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

namespace MainApp.Controllers
{

    public class EnrolledProgramController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public EnrolledProgramController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {

            EnrolledProgram model = new EnrolledProgram();
            return View(model);
        }

        public IActionResult Create()
        {
            EnrolledProgram model = new EnrolledProgram();

            model.ICollege = _context.Colleges
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.CollegeID.ToString(),
                                   Text = a.CollegeName
                               }).OrderBy(a => a.Value)
                               .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrolledProgram viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserID = User.Identity.Name;
                _context.Add(viewModel);
                await _context.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
            //return RedirectToAction(nameof(Edit), new { CollegeID = viewModel.CollegeID });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int ProgramID)
        {
            if (ProgramID == 0)
            {
                return NotFound();
            }

            EnrolledProgram EnrolledProgramObj = await Queryable.Where<EnrolledProgram>(_context.EnrolledPrograms, (System.Linq.Expressions.Expression<Func<EnrolledProgram, bool>>)(x => (bool)(x.ProgramID == ProgramID))).FirstOrDefaultAsync();
            EnrolledProgramObj.ICollege = _context.Colleges
                            .Select(a => new SelectListItem()
                            {
                                Value = a.CollegeID.ToString(),
                                Text = a.CollegeName
                            }).OrderBy(a => a.Value)
                            .ToList();

            return View(EnrolledProgramObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EnrolledProgram viewModel)
        {
            var currentUserID = User.Identity.Name;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogInformation("Edit File:" + e.Message);
                }
                return RedirectToAction(nameof(Edit), new { ProgramID = viewModel.ProgramID });
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEnrolledProgram(int ProgramID)
        {
            string name = string.Empty;
            EnrolledProgram model = new EnrolledProgram();
            if (ProgramID != 0)
            {
                model = await _context.EnrolledPrograms
                 .FirstOrDefaultAsync(m => m.ProgramID == ProgramID);
                if (model != null)
                {
                    name = model.ProgramName;
                }
            }
            return PartialView("_DeleteEnrolledProgram", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEnrolledProgram(IFormCollection form)
        {
            var objCons = await _context.EnrolledPrograms.FirstOrDefaultAsync(m => m.ProgramID == Convert.ToInt32(form["ProgramID"]));

            _logger.LogInformation($"Delete Program of {objCons.ProgramName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.ProgramName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting Program of " + objCons.ProgramName);
            }

            _context.EnrolledPrograms.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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


                var objData = (from t in _context.EnrolledPrograms
                               join x in _context.Colleges on t.CollegeID equals x.CollegeID into tl
                               from w in tl.DefaultIfEmpty()
                               orderby t.ProgramName descending
                               select new
                               {
                                   CollegeName = w == null ? "(No College)" : w.CollegeName.Trim(),
                                   ProgramID = t.ProgramID,
                                   ProgramCode = t.ProgramCode,
                                   ProgramName = t.ProgramName,
                                   NumberofSemester = t.NumberofSemester
                               });
                // Getting all Colleges data  

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.ProgramCode.Contains(searchValue) || m.ProgramName.ToString().Contains(searchValue));
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
    }
}