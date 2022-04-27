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

namespace MainApp.Controllers {

    public class StudentProfileController : Controller {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public StudentProfileController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger, UserManager<IdentityUser> userManager) {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index() {

            StudentProfile model = new StudentProfile();
            return View(model);
        }

        public IActionResult Create() {
            StudentProfile model = new StudentProfile();

            ViewBag.Cities = _context.Cities
                .Select(c => new SelectListItem {
                    Value = c.CityID.ToString(),
                    Text = c.CityName
                });

            ViewBag.Provinces = _context.Provinces
                .Select(p => new SelectListItem {
                    Value = p.ProvinceID.ToString(),
                    Text = p.ProvinceName
                });

            ViewBag.EnrolledPrograms = _context.EnrolledPrograms
                .Select(ep => new SelectListItem {
                    Value = ep.ProgramID.ToString(),
                    Text = ep.ProgramName
                });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentProfile viewModel) {
            if (ModelState.IsValid) {
                var currentUserID = User.Identity.Name;
                _context.Add(viewModel);
                await _context.SaveChangesAsync();
            } else {
                return RedirectToAction(nameof(Create));
            }
            //return RedirectToAction(nameof(Edit), new { CollegeID = viewModel.CollegeID });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int StudentProfileId) {
            if (StudentProfileId == 0) {
                return NotFound();
            }

            StudentProfile StudentProfileObj = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.StudentID == StudentProfileId);

            ViewBag.Cities = _context.Cities
                .Select(c => new SelectListItem {
                    Value = c.CityID.ToString(),
                    Text = c.CityName
                });

            ViewBag.Provinces = _context.Provinces
                .Select(p => new SelectListItem {
                    Value = p.ProvinceID.ToString(),
                    Text = p.ProvinceName
                });

            ViewBag.EnrolledPrograms = _context.EnrolledPrograms
                .Select(ep => new SelectListItem {
                    Value = ep.ProgramID.ToString(),
                    Text = ep.ProgramName
                });

            return View(StudentProfileObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StudentProfile viewModel) {
            var currentUserID = User.Identity.Name;
            if (ModelState.IsValid) {
                try {
                    _context.Update(viewModel);
                    await _context.SaveChangesAsync();
                } catch (Exception e) {
                    _logger.LogInformation("Edit File:" + e.Message);
                }
                return RedirectToAction(nameof(Edit), new { StudentProfileId = viewModel.StudentID });
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStudentProfile(int StudentProfileId) {
            if (StudentProfileId != 0) {
                var model = await _context.StudentProfiles
                    .FirstOrDefaultAsync(cs => cs.StudentID == StudentProfileId);
                if (model != null) {
                    return PartialView("_DeleteStudentProfile", model);
                }
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteStudentProfile(IFormCollection form) {
            var objCons = await _context.StudentProfiles.FirstOrDefaultAsync(m => m.StudentID == Convert.ToInt32(form["StudentProfileId"]));

            _logger.LogInformation($"Delete Program of {objCons.StudentID}");

            try {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.StudentID.ToString()));
            } catch (Exception) {
                _logger.LogInformation("Error deleting Program of " + objCons.StudentID);
            }

            _context.StudentProfiles.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LoadData() {
            try {

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

                // Getting all Colleges data  
                var objData = _context.StudentProfiles
                    .Select(sp => new { 
                        ProgramName = sp.ProgramID == 0 ? "(No Program)" : sp.ProgramID.ToString(),
                        CityName = sp.CityID == 0 ? "(No City)" : sp.CityID.ToString(),
                        ProvinceName = sp.ProvinceID == 0 ? "(No Province)" : sp.ProvinceID.ToString(),
						sp.StudentID,
						sp.LastName,
						sp.FirstName,
                        sp.MiddleName,
                        sp.Gender,
                        sp.BirthDate,
                        sp.MobileNumber,
                        sp.EmailAddress
                    });

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection))) {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue)) {
                    objData = objData
                        .Where(sp => 
                            sp.LastName.Contains(searchValue) || 
                            sp.FirstName.Contains(searchValue) ||
                            sp.MiddleName.Contains(searchValue) ||
                            sp.CityName.Contains(searchValue) ||
                            sp.ProgramName.Contains(searchValue) ||
                            sp.ProvinceName.Contains(searchValue) ||
                            sp.EmailAddress.Contains(searchValue)
                        );
                }

                //total number of rows count   
                recordsTotal = objData.Count();
                //Paging   
                var data = await objData.Skip(skip).Take(pageSize).ToListAsync();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            } catch (Exception) {
                throw;
            }
        }
    }
}