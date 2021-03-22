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
    public class CourseScheduleController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public CourseScheduleController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
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
                var objData = (from t in _context.CourseSchedules
                                join cid in _context.Courses on t.CourseID equals cid.CourseID into tl
                                from w in tl.DefaultIfEmpty()
                                join ayid in _context.Semesters on t.AYSemID equals ayid.AYSemID into tm
                                from a in tm.DefaultIfEmpty()
                                join rid in _context.Rooms on t.RoomID equals rid.RoomID into tn
                                from b in tn.DefaultIfEmpty()
                                join cid in _context.Faculties on t.FacultyID equals cid.FacultyID into tf
                                from d in tf.DefaultIfEmpty()
                                orderby t.ScheduleCourseID descending
                                select new
                                {
                                   ScheduleCourseID = t.ScheduleCourseID,
                                   CourseID = t.CourseID,
                                   CourseName = w == null ? ("No CourseName") : w.CourseCode,
                                   AYSemID = t.AYSemID,
                                   AcademicYear = a == null ? ("No AYSemName") : a.AcademicYear,
                                   FacultyID = t.FacultyID,
                                   FacultyName = d == null ? ("No FacultyName") : d.FacultyName,
                                   Days = t.Days,
                                   TimeFrom = t.TimeFrom,
                                   TimeTo = t.TimeTo,
                                   RoomID = t.RoomID,
                                   RoomName = b == null ? ("No RoomName") : b.RoomName,
                                });

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.AcademicYear.Contains(searchValue));
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
        public async Task<IActionResult> AddEditScheduleCourse(int ScheduleCourseID)
        {
            Guid gid = Guid.NewGuid();
            CourseSchedule model = new CourseSchedule();
            model.ICourse = _context.Courses
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.CourseID.ToString(),
                                   Text = a.CourseCode
                               }).OrderBy(a => a.Value)
                               .ToList();
            model.IRoom = _context.Rooms
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.RoomID.ToString(),
                                   Text = a.RoomName
                               }).OrderBy(a => a.Value)
                               .ToList();
            model.ISemester = _context.Semesters
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.AYSemID.ToString(),
                                   Text = a.AcademicYear
                               }).OrderBy(a => a.Value)
                               .ToList();
            model.IFaculty = _context.Faculties
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.FacultyID.ToString(),
                                   Text = a.FacultyName
                               }).OrderBy(a => a.Value)
                               .ToList();
            string userID = User.Identity.Name;

            if (ScheduleCourseID != 0)
            {
                model = await _context.CourseSchedules
                 .FirstOrDefaultAsync(m => m.ScheduleCourseID == ScheduleCourseID);
            }

            return PartialView("_AddEditScheduleCourse", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditScheduleCourse(CourseSchedule model)
        {
            if (ModelState.IsValid)
            {
                if (model.ScheduleCourseID == 0)
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
        public async Task<IActionResult> DeleteScheduleCourse(int ScheduleCourseID)
        {
            int name = 0;
            CourseSchedule model = new CourseSchedule();
            if (ScheduleCourseID != 0)
            {

                model = await _context.CourseSchedules
                 .FirstOrDefaultAsync(m => m.ScheduleCourseID == ScheduleCourseID);
                if (model != null)
                {
                    name = model.ScheduleCourseID;
                }
            }
            return PartialView("_DeleteScheduleCourse", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCourseSchedule(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["CourseScheduleID"]);
            var objCons = await _context.CourseSchedules
                 .FirstOrDefaultAsync(m => m.ScheduleCourseID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.ScheduleCourseID}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByIdAsync(objCons.ScheduleCourseID.ToString()));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.ScheduleCourseID);
            }


            _context.CourseSchedules.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool CourseScheduleExists(int id)
        {
            return _context.CourseSchedules.Any(e => e.ScheduleCourseID == id);
        }
    }
}
