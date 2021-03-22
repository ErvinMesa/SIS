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
    public class RoomController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public RoomController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
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
                var objData = (from t in _context.Rooms
                               join b in _context.Buildings on t.BuildingID equals b.BuildingID into tb
                               from w in tb.DefaultIfEmpty()
                               orderby t.RoomName descending
                               select new
                               {
                                   RoomID = t.RoomID,
                                   BuildingID = t.BuildingID,
                                   BuildingName = w == null ? "(No College)" : w.BuildingName.Trim(),
                                   RoomCode = t.RoomCode,
                                   RoomName = t.RoomName,
                                   Capacity = t.Capacity,
                               });

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.RoomName.Contains(searchValue));
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
        public async Task<IActionResult> AddEditRoom(int RoomID)
        {
            Guid gid = Guid.NewGuid();
            Room model = new Room();
            model.IBuilding = _context.Buildings
                               .Select(a => new SelectListItem()
                               {
                                   Value = a.BuildingID.ToString(),
                                   Text = a.BuildingName
                               }).OrderBy(a => a.Value)
                               .ToList();
            string userID = User.Identity.Name;

            if (RoomID != 0)
            {
                model = await _context.Rooms
                 .FirstOrDefaultAsync(m => m.RoomID == RoomID);
            }

            return PartialView("_AddEditRoom", model);
        }


        [HttpPost]
        public async Task<IActionResult> AddEditRoom(Room model)
        {
            if (ModelState.IsValid)
            {
                if (model.RoomID == 0)
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
        public async Task<IActionResult> DeleteRoom(int RoomID)
        {
            string name = string.Empty;
            Room model = new Room();
            if (RoomID != 0)
            {

                model = await _context.Rooms
                 .FirstOrDefaultAsync(m => m.RoomID == RoomID);
                if (model != null)
                {
                    name = model.RoomName;
                }
            }
            return PartialView("_DeleteRoom", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRoom(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["RoomID"]);
            var objCons = await _context.Rooms
                 .FirstOrDefaultAsync(m => m.RoomID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.RoomName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.RoomName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.RoomName);
            }


            _context.Rooms.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RoomID == id);
        }
    }
}
