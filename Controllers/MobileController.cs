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
        private ICityService _cityService;
        private IProvinceService _provinceService;
        private IFacultyService _facultyService;
        private ISemesterService _semesterService;
        private ICourseService _courseService;

        private IUserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public class UserAccount
        {
            public string UserName { get; set; }
            public string Password { get; set; }

        }


        public MobileController(ICourseService courseService, ISemesterService semesterService, IFacultyService facultyService, IProvinceService provinceService, ICityService cityService, ICollegeService collegeService, IBuildingService buildingService,
            IUserService userService, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _collegeService = collegeService;
            _userService = userService;
            _buildingService = buildingService;
            _cityService = cityService;
            _provinceService = provinceService;
            _facultyService = facultyService;
            _semesterService = semesterService;
            _courseService = courseService;
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

        #region Reg-CityService

        [HttpPost, Route("GetAllCity"), Produces("application/json")]
        public IActionResult GetAllCity([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var city = _cityService.GetAll();
                        return Ok(city);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }

            }
        }


        [HttpPost, Route("GetcityInfo/{id}"), Produces("application/json")]
        public IActionResult GetcityInfo(int id, [FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var city = _cityService.GetCityInfo(id);
                        return Ok(city);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("CreateCity"), Produces("application/json")]
        public IActionResult CreateCity([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                City model = data["City"].ToObject<City>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var city = _cityService.Create(model);
                        return Ok(city);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("UpdateCity"), Produces("application/json")]
        public IActionResult UpdateCity([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                City model = data["City"].ToObject<City>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var city = _cityService.Update(model);
                        return Ok(city);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("DeleteCity"), Produces("application/json")]
        public IActionResult DeleteCity([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                City model = data["City"].ToObject<City>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var city = _cityService.Delete(model);
                        return Ok("Deleted");
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        #endregion

        #region Reg-ProvinceService

        [HttpPost, Route("GetAllProvince"), Produces("application/json")]
        public IActionResult GetAllProvince([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var province = _provinceService.GetAll();
                        return Ok(province);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }

            }
        }


        [HttpPost, Route("GetprovinceInfo/{id}"), Produces("application/json")]
        public IActionResult GetprovinceInfo(int id, [FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var province = _provinceService.GetProvinceInfo(id);
                        return Ok(province);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("CreateProvince"), Produces("application/json")]
        public IActionResult CreateProvince([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Province model = data["Province"].ToObject<Province>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var province = _provinceService.Create(model);
                        return Ok(province);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("UpdateProvince"), Produces("application/json")]
        public IActionResult UpdateProvince([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Province model = data["Province"].ToObject<Province>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var province = _provinceService.Update(model);
                        return Ok(province);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("DeleteProvince"), Produces("application/json")]
        public IActionResult DeleteProvince([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Province model = data["Province"].ToObject<Province>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var province = _provinceService.Delete(model);
                        return Ok("Deleted");
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        #endregion

        #region Reg-FacultyService

        [HttpPost, Route("GetAllFaculty"), Produces("application/json")]
        public IActionResult GetAllFaculty([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var faculty = _facultyService.GetAll();
                        return Ok(faculty);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }

            }
        }


        [HttpPost, Route("GetfacultyInfo/{id}"), Produces("application/json")]
        public IActionResult GetfacultyInfo(int id, [FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var faculty = _facultyService.GetFacultyInfo(id);
                        return Ok(faculty);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("CreateFaculty"), Produces("application/json")]
        public IActionResult CreateFaculty([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Faculty model = data["Faculty"].ToObject<Faculty>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var faculty = _facultyService.Create(model);
                        return Ok(faculty);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("UpdateFaculty"), Produces("application/json")]
        public IActionResult UpdateFaculty([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Faculty model = data["Faculty"].ToObject<Faculty>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var faculty = _facultyService.Update(model);
                        return Ok(faculty);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("DeleteFaculty"), Produces("application/json")]
        public IActionResult DeleteFaculty([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Faculty model = data["Faculty"].ToObject<Faculty>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var faculty = _facultyService.Delete(model);
                        return Ok("Deleted");
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        #endregion

        #region Reg-SemesterService

        [HttpPost, Route("GetAllSemester"), Produces("application/json")]
        public IActionResult GetAllSemester([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var semester = _semesterService.GetAll();
                        return Ok(semester);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }

            }
        }


        [HttpPost, Route("GetsemesterInfo/{id}"), Produces("application/json")]
        public IActionResult GetsemesterInfo(int id, [FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var semester = _semesterService.GetSemesterInfo(id);
                        return Ok(semester);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("CreateSemester"), Produces("application/json")]
        public IActionResult CreateSemester([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Semester model = data["Semester"].ToObject<Semester>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var semester = _semesterService.Create(model);
                        return Ok(semester);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("UpdateSemester"), Produces("application/json")]
        public IActionResult UpdateSemester([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Semester model = data["Semester"].ToObject<Semester>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var semester = _semesterService.Update(model);
                        return Ok(semester);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("DeleteSemester"), Produces("application/json")]
        public IActionResult DeleteSemester([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Semester model = data["Semester"].ToObject<Semester>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var semester = _semesterService.Delete(model);
                        return Ok("Deleted");
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        #endregion

        #region Reg-CourseService

        [HttpPost, Route("GetAllCourse"), Produces("application/json")]
        public IActionResult GetAllCourse([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var course = _courseService.GetAll();
                        return Ok(course);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }

            }
        }


        [HttpPost, Route("GetcourseInfo/{id}"), Produces("application/json")]
        public IActionResult GetcourseInfo(int id, [FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var course = _courseService.GetCourseInfo(id);
                        return Ok(course);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("CreateCourse"), Produces("application/json")]
        public IActionResult CreateCourse([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Course model = data["Course"].ToObject<Course>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var course = _courseService.Create(model);
                        return Ok(course);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("UpdateCourse"), Produces("application/json")]
        public IActionResult UpdateCourse([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Course model = data["Course"].ToObject<Course>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var course = _courseService.Update(model);
                        return Ok(course);
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        [HttpPost, Route("DeleteCourse"), Produces("application/json")]
        public IActionResult DeleteCourse([FromBody] JObject data)
        {
            {
                UserAccount UserModel = data["UserAccount"].ToObject<UserAccount>();
                Course model = data["Course"].ToObject<Course>();

                var result = _signInManager.PasswordSignInAsync(UserModel.UserName, UserModel.Password, false, lockoutOnFailure: false);
                if (result != null)
                {
                    {
                        var course = _courseService.Delete(model);
                        return Ok("Deleted");
                    }
                }
                else
                {
                    {
                        return Ok("Authentication Issue.");
                    }
                }
            }
        }

        #endregion

    }
}
