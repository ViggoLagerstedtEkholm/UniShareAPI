using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Requests.Settings;
using UniShareAPI.Models.DTO.Response.Settings;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public SettingsController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        
        [HttpPost]
        [Route("account/update")]
        public async Task<IActionResult> UpdateAccountSettings([FromBody] AccountSettingsRequest accountSettingsRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

                var userWithUsername = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == accountSettingsRequest.Username);

                if(userWithUsername == null)
                {
                    user.UserName = accountSettingsRequest.Username;
                }
                else
                {
                    return BadRequest();
                }

                user.Firstname = accountSettingsRequest.Firstname;
                user.Lastname = accountSettingsRequest.Lastname;
                user.Description = accountSettingsRequest.Description;
                user.Age = accountSettingsRequest.Age;

                await _userManager.UpdateAsync(user);

                return Ok();
            }
            return BadRequest();
        }
        

        [HttpPost]
        [Route("password/update")]
        public async Task UpdatePassword([FromBody] PasswordRequest passwordRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            await _userManager.ChangePasswordAsync(user, passwordRequest.CurrentPassword, passwordRequest.NewPassword);
        }

        [HttpGet]
        [Route("account")]
        public async Task<IActionResult> GetAccountSettings()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            if(user == null)
            {
                return BadRequest("Eyo wut");
            }

            var userSettings = new AccountSettingsResponse()
            {
                Username = user.UserName,
                Description = user.Description,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Age = user.Age
            };

            return Ok(userSettings);
        }

        [HttpGet]
        [Route("handles")]
        public async Task<IActionResult> GetHandles()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            if (user == null)
            {
                return BadRequest("Eyo wut");
            }

            var userSettings = new AccountHandlesResponse()
            {
                GitHub = user.GitHub,
                LinkedIn = user.LinkedIn
            };

            return Ok(userSettings);
        }

        [HttpPost]
        [Route("handles/update")]
        public async Task UpdateHandles([FromBody] HandlesSettingsRequest handlesRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            user.GitHub = handlesRequest.Github;
            user.LinkedIn = handlesRequest.LinkedIn;
            await _userManager.UpdateAsync(user);
        }

        [HttpPost]
        [Route("handles/delete/github")]
        public async Task UpdateGitHub()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            user.GitHub = "";
            await _userManager.UpdateAsync(user);
        }

        [HttpPost]
        [Route("handles/delete/linkedin")]
        public async Task UpdateLinkedin()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            user.LinkedIn = "";
            await _userManager.UpdateAsync(user);
        }

        [HttpGet]
        [Route("degrees")]
        public IActionResult GetDegrees()
        {
            var degrees = _appDbContext.Degrees.Where(x => x.User.Id == HttpContext.GetUserId());
            return Ok(degrees);
        }


        [HttpGet]
        [Route("degrees/active")]
        public async Task<IActionResult> GetActiveDegreeAsync()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            if (user.ActiveDegreeId == null)
            {
                return NotFound("No active degree at this ID was found.");
            }
            else
            {
                return Ok(user.ActiveDegreeId);
            }
        }

        [HttpPost]
        [Route("update/active")]
        public async Task<IActionResult> UpdateActiveDegree([FromForm] UpdatedActiveDegree updatedActiveDegree)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var degree = await _appDbContext.Degrees.FirstOrDefaultAsync(x => x.Id == updatedActiveDegree.ActiveDegreeId);

            if(updatedActiveDegree.ActiveDegreeId == -1)
            {
                user.ActiveDegreeId = null;
                _appDbContext.User.Update(user);
                await _appDbContext.SaveChangesAsync();
            }

            if(degree != null && degree.User.Id == user.Id)
            {
                user.ActiveDegreeId = updatedActiveDegree.ActiveDegreeId;

                _appDbContext.User.Update(user);
                await _appDbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
