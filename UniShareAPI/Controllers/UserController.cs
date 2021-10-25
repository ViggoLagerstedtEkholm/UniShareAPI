using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniShareAPI.Models.Relations;
using UniShareAPI.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;
using UniShareAPI.Models.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public UserController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserViewModel()
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Image = null,
                Username = user.UserName,
                Visits = user.Visits,
                Id = user.Id,
                LastOnline = user.LastSeenDate
            };

            return Ok(userModel);
        }

        [HttpPost]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Could not delete user.");
            }

        }
    }
}
