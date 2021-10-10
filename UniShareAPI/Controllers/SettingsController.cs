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

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SettingsController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public SettingsController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("update/account")]
        public void UpdateAccount([FromBody] UpdateSettingsRequest settings)
        {
            //Update settings.
        }

        [HttpGet]
        [Route("username/available")]
        public void CheckUsernameAvailability(string username)
        {
            //Check username availability.
        }

        [HttpGet]
        [Route("email/available")]
        public void CheckEmailAvailability(string email)
        {
            //Check email availability.
        }

        [HttpPost]
        [Route("update/password")]
        public void UpdatePassword([FromBody] PasswordRequest password)
        {
            //Update password.
        }

        [HttpPost]
        [Route("update/active/degree")]
        public void UpdateActiveDegree([FromBody] string degreeId)
        {
            //Update active degree.
        }

        [HttpPost]
        [Route("delete/account")]
        public void DeleteAccount([FromBody] string degreeId)
        {
            //Delete account by ID.
        }

        [HttpPost]
        [Route("delete/github")]
        public void DeleteGithub([FromBody] string userId)
        {
            //Delete github.
        }

        [HttpPost]
        [Route("delete/linkedin")]
        public void DeleteLinkedIn([FromBody] string userId)
        {
            //Delete linkedin.
        }

        [HttpPost]
        [Route("update/handles")]
        public void UpdateHandles([FromBody] string userId)
        {
            //Delete linkedin.
        }

        [HttpGet]
        [Route("handles")]
        public void GetHandles([FromBody] string userId)
        {
            //Get handles.
        }

        [HttpGet]
        [Route("fetch")]
        public void Fetch([FromBody] string userId)
        {
            //Get settings.
        }
    }
}
