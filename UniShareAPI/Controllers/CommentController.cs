using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.Relations;
using UniShareAPI.Models.Viewmodels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public CommentController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet("{id}")]
        public CommentViewmodel GetComment(int id)
        {
            var comment = new CommentViewmodel()
            {
                Text = "Comment!",
                Date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Author = "AUTHOR ID",
                Profile = "PROFILE ID"
            };

            return comment;
        }

        [HttpPost]
        [Route("post")]
        public void PostComment([FromBody] CommentRequest comment)
        {
            //Save comment
        }

        [HttpDelete("{id}")]
        public void DeleteComment(int id)
        {
            //Check if owner before deleting.
            //Delete comment
        }
    }
}
