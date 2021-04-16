using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using SIS.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MobileController : ControllerBase
    {
        private ICollegeService _collegeService;
        private IBuildingService _buildingService;

        private IUserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public class UserAccount
        {
            public string UserName { get; set; }
            public string Password { get; set; }

        }


        public MobileController(ICollegeService collegeService, IBuildingService buildingService,
            IUserService userService, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _collegeService = collegeService;
            _userService = userService;
            _buildingService = buildingService;
        }

        #region College
        [HttpPost, Route("GetAllCollege"), Produces("application/json")]
        public IActionResult GetAllCollege([FromBody] JObject data)
        {
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null)
            {
                var college = _collegeService.GetAll();
                return Ok(college);
            }
            else
            {
                return Ok("Authentication Issue.");
            }

        }

        [HttpPost, Route("GetCollegeInfo/{id}"), Produces("application/json")]
        public IActionResult GetCollegeInfo(int id, [FromBody] JObject data)
        {
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null)
            {
                var college = _collegeService.GetCollegeInfo(id);
                return Ok(college);
            }
            else
            {
                return Ok("Authentication Issue.");
            }
        }

        [HttpPost, Route("CreateCollege"), Produces("application/json")]
        public IActionResult CreateCollege([FromBody] JObject data)
        {
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            College model = data["College"].ToObject<College>();

            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null)
            {
                var college = _collegeService.Create(model);
                return Ok(college);
            }
            else
            {
                return Ok("Authentication Issue.");
            }
        }

        [HttpPost, Route("UpdateCollege"), Produces("application/json")]
        public IActionResult UpdateCollege([FromBody] JObject data)
        {
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            College model = data["College"].ToObject<College>();

            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null)
            {
                var college = _collegeService.Update(model);
                return Ok(college);
            }
            else
            {
                return Ok("Authentication Issue.");
            }
        }
        #endregion

        #region Reg-BuildingService
        [HttpPost, Route("GetAllBuilding"), Produces("application/json")]
        public IActionResult GetAllBuilding([FromBody] JObject data) 
        { 
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null) 
            { 
                var college = _buildingService.GetAll();
                return Ok(college);
            } 
            else 
            { 
                return Ok("Authentication Issue."); 
            } 
        }
        [HttpPost, Route("GetBuildingInfo/{id}"), Produces("application/json")]
        public IActionResult GetBuildingInfo(int id, [FromBody] JObject data) 
        { 
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null) 
            { 
                var building = _buildingService.GetBuildingInfo(id);
                return Ok(building);
            } 
            else 
            { 
                return Ok("Authentication Issue."); 
            } 
        }
        [HttpPost, Route("CreateBuilding"), Produces("application/json")]
        public IActionResult CreateBuilding([FromBody] JObject data) 
        { 
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            Building model = data["Building"].ToObject<Building>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null) 
            { 
                var building = _buildingService.Create(model);
                return Ok(building); 
            }
            else 
            { 
                return Ok("Authentication Issue."); 
            } 
        }

        [HttpPost, Route("UpdateBuilding"), Produces("application/json")]
        public IActionResult UpdateBuilding([FromBody] JObject data) 
        { 
            UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
            Building model = data["Building"].ToObject<Building>();
            var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
            if (result != null) 
            { 
                var building = _buildingService.Update(model);
                return Ok(building); 
            } 
            else 
            { 
                return Ok("Authentication Issue."); 
            } 
        }

        #endregion
    }
}
