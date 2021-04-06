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
using WebApi.Services;
using System.IO;
using Microsoft.Extensions.Options;
using WebApi.Helpers;
using System.Text.Json;

namespace SIS.Controllers
{

    [Authorize(Roles = "Registrar, Staff")]
    public class CollegeController : Controller
    {

        private ICollegeService _collegeService;
        private IEmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication 
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication 
        private readonly ILogger<RegisterModel> _logger;

        public CollegeController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger, UserManager<IdentityUser> userManager, ICollegeService collegeService,IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _collegeService = collegeService;
            _emailService = emailService;
            _appSettings = appSettings.Value;
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
            model.FileStamp = gid.ToString();
            model.RecognizeDate = DateTime.Now;
            model.IsActive = true;

            if (CollegeID != 0)
            {
                //model = await _context.Colleges
                // .FirstOrDefaultAsync(m => m.CollegeID == CollegeID);
                model = _collegeService.GetCollegeInfo(CollegeID);


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
                    //_context.Add(model);

                    _collegeService.Create(model);
                    _emailService.Send(_appSettings.SmtpUser, "alvinmananquil@gmail.com", "New College", JsonSerializer.Serialize(model),"");
                }
                else
                {
                    //_context.Update(model);
                    _collegeService.Update(model);
                    var tempObj = await _context.EmailTemplates.FirstOrDefaultAsync(m => m.SubjectContent == "Update College");

                    string emailSubject = "";
                    string emailContent = "";
                    if (tempObj != null)
                    {
                        emailSubject = tempObj.SubjectContent;
                        emailContent = tempObj.TemplateContent;
                        emailContent = emailContent.Replace("{CollegeCode}", model.CollegeCode);
                        emailContent = emailContent.Replace("{CollegeName}", model.CollegeName);
                        emailContent = emailContent.Replace("{NameofDean}", model.NameofDean);
                        _emailService.Send(_appSettings.SmtpUser, tempObj.EmailTo, emailSubject, emailContent,tempObj.EmailCC);
                    }
                    else
                    {
                        _emailService.Send(_appSettings.SmtpUser, tempObj.EmailTo, "Edit College", JsonSerializer.Serialize(model),tempObj.EmailCC);
                    }
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


        [HttpGet]
        public async Task<IActionResult> UploadLogo(int CollegeID)
        {
            Guid gid = Guid.NewGuid();

            College model = new College();
            string userID = User.Identity.Name;
            if (model.FileStamp != "")
            {
                model.FileStamp = gid.ToString();
            }
            return View(model);
        }


        public async Task<IActionResult> UploadLogo(List<IFormFile> files, College viewModel)
        {
            string userId = HttpContext.User.Identity.Name;

            var folderPath = $"wwwroot\\AppPhoto";

            if (Directory.Exists(folderPath) == false)
            {
                DirectoryInfo di = Directory.CreateDirectory(folderPath);
            }

            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var file in Request.Form.Files)
            {
                //get uploaded file name: true to create temp name, false to get real name
                var model = _collegeService.GetCollegeInfo(viewModel.CollegeID);


                var fileType = Path.GetExtension(file.FileName);
                var fileName = model.FileStamp + fileType;
                model.Logo = fileName;
                _collegeService.Update(model);

                if (file.Length > 0)
                {
                    if (fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".bmp"))
                    {
                        // optional : server side resize create image with watermark
                        // these steps requires LazZiya.ImageResize package from nuget.org
                        // upload and save files to upload folder
                        using (var stream = new FileStream($"wwwroot\\AppPhoto\\{fileName}", FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                }
            }
            return Ok(new { count = files.Count, size, filePaths });
        }
        private bool CollegeExists(int id)
        {
            return _context.Colleges.Any(e => e.CollegeID == id);
        }
    }
}
