using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Configuration;
using UniShareAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public HomeController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        // GET: api/<HomeController>
        [HttpGet]
        [Route("user/{id}")]
        public IActionResult GetUser(string id)
        {
            return Ok("ID sent: " + id);
        }

        // GET api/<HomeController>/5
        [HttpGet("top/courses")]
        public string GetCourses(int id)
        {
            return "value";
        }

        [HttpGet("top/forums")]
        public string GetForums(int id)
        {
            //Get all forums ordered by views.
            return "value";
        }

        [HttpGet("top/statistics")]
        public string Statistics(int id)
        {
            //Sum of users
            //Sum of courses
            //Sum of forums
            return "value";
        }
    }
}
