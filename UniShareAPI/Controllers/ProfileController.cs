using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models;
using UniShareAPI.Models.Viewmodels;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public ProfileController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public string GetProfile(int id)
        {
            return "Get profile: " + id;
        }

        [HttpPost]
        [Route("user/append/{id}")]
        public IActionResult AppendVisit(string id)
        {
            return Ok("Append user by ID: " + id);
        }


        [HttpPost]
        [Route("user/upload/image/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Upload(string id)
        {
            return Ok("Uploading image user ID: " + id);
        }

        [HttpGet("comments")]
        public IEnumerable<CommentViewmodel> GetComments([FromBody] int id)
        {
            List<CommentViewmodel> comments = new()
            {
                new CommentViewmodel()
                {
                    Text = "Comment!",
                    Date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Author = "AUTHOR ID",
                    Profile = "PROFILE ID"
                }
            };

            return comments;
        }

        [HttpPost]
        [Route("remove/course/degree}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RemoveCourseFromDegree(string id)
        {
            return Ok("Remove course from degree: " + id);
        }
    }
}
