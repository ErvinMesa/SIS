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
using WebApi.Models;

namespace SIS.Controllers
{

    [Authorize(Roles = "Registrar, Staff")]
    public class EmailTemplateController : Controller
    {
        private IEmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // used for authentication
        private readonly SignInManager<IdentityUser> _signInManager; // used for authentication
        private readonly ILogger<RegisterModel> _logger;

        public EmailTemplateController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger, UserManager<IdentityUser> userManager, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailService = emailService;
            _appSettings = appSettings.Value;
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
                var objData = (from t in _context.EmailTemplates
                            select new
                            {
                                TemplateID = t.TemplateID,
                                TemplateName = t.TemplateName,
                                SubjectContent = t.SubjectContent,
                                EmailTo = t.EmailTo,
                                EmailCC = t.EmailCC
                            });

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    objData = objData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    objData = objData.Where(m => m.TemplateName.Contains(searchValue));
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
        public IActionResult Index()
        {

            EmailTemplate model = new EmailTemplate();
            return View(model);
        }

        public IActionResult Create()
        {
            EmailTemplate model = new EmailTemplate();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailTemplate viewModel)
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
        public async Task<IActionResult> Edit(int TemplateID)
        {
            if (TemplateID == 0)
            {
                return NotFound();
            }

            EmailTemplate EmailTemplateObj = await Queryable.Where<EmailTemplate>(_context.EmailTemplates, (System.Linq.Expressions.Expression<Func<EmailTemplate, bool>>)(x => (bool)(x.TemplateID == TemplateID))).FirstOrDefaultAsync();

            return View(EmailTemplateObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailTemplate viewModel)
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
                return RedirectToAction(nameof(Edit), new { TemplateID = viewModel.TemplateID });
            }
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteEmailTemplate(int TemplateID)
        {
            string name = string.Empty;
            EmailTemplate model = new EmailTemplate();
            if (TemplateID != 0)
            {

                model = await _context.EmailTemplates
                .FirstOrDefaultAsync(m => m.TemplateID == TemplateID);
                if (model != null)
                {
                    name = model.TemplateName;
                }
            }
            return PartialView("_DeleteEmailTemplate", model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEmailTemplate(IFormCollection form)
        {
            int RecordID = Convert.ToInt32(form["TemplateID"]);
            var objCons = await _context.EmailTemplates
                .FirstOrDefaultAsync(m => m.TemplateID == RecordID);
            _logger.LogInformation($"Delete Account of {objCons.TemplateName}");

            try
            {
                await _userManager.DeleteAsync(await _userManager.FindByNameAsync(objCons.TemplateName));
            }
            catch (Exception e)
            {
                _logger.LogInformation("Error deleting user account of " + objCons.TemplateName);
            }


            _context.EmailTemplates.Remove(objCons);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        private bool EmailTemplateExists(int id)
        {
            return _context.EmailTemplates.Any(e => e.TemplateID == id);
        }
    }
}