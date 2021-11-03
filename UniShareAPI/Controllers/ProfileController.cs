using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;
using UniShareAPI.Models.Viewmodels;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public static IWebHostEnvironment _webHostEnvironment;
        public ProfileController(UserManager<User> userManager,
            AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetUserProfileAsync(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserProfileViewModel()
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Image = null,
                Username = user.UserName,
                Visits = user.Visits,
                Id = user.Id,
                LastOnline = user.LastSeenDate,
                Joined = user.Joined,
                LinkedIn = user.LinkedIn,
                GitHub = user.GitHub,
                Description = user.Description,
                Age = user.Age
            };

            return Ok(userModel);
        }

        [HttpPost]
        [Route("append/{username}")]
        public async Task<IActionResult> AppendVisitAsync(string username)
        {
            var appendUser = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if(appendUser != null)
            {
                appendUser.Visits++;
                await _userManager.UpdateAsync(appendUser);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("image/upload")]
        public async Task<IActionResult> UploadImageAsync([FromForm] FileUpload fileUpload)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            try
            {
                using var stream = new MemoryStream();
                fileUpload.file.CopyTo(stream);
                var fileBytes = stream.ToArray();
                user.Image = fileBytes;
                await _userManager.UpdateAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("image/get/{username}")]
        public async Task<IActionResult> GetImageAsync(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if(user == null)
            {
                return BadRequest("No such user.");
            }

            if ( user.Image != null)
            {
                user.Image = user.Image;
            }

            return Ok(user.Image);
        }
    }
}
