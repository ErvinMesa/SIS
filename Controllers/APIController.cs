using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {
        private ICollegeService _collegeService;

        public APIController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        [HttpGet("GetAllCollege")]
        public IActionResult GetAllCollege()
        {
            var college = _collegeService.GetAll();
            return Ok(college);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var college = _collegeService.GetCollegeInfo(id);
            if (college == null) return NotFound();

            return Ok(college);
        }

    }
}
