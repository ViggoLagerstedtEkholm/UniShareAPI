using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public DegreeController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Get degree.
            return "Get degree: " + id;
        }

        [HttpGet]
        [Route("active/degree")]
        public string GetActiveDegree(int id)
        {
            //Get active degree.
            return "Get active degree: " + id;
        }

        [HttpGet]
        [Route("active/degree/settings")]
        public string GetDegreeSettings(int id)
        {
            //Get degree settings.
            return "Get degree settings: " + id;
        }

        [HttpPost]
        [Route("update")]
        public string Update(int id)
        {
            //Update degree.
            return "Update degree: " + id;
        }

        [HttpPost]
        [Route("upload")]
        public string Upload(int id)
        {
            //Upload degree.
            return "Update degree: " + id;
        }

        [HttpPost]
        [Route("upload")]
        public string Delete(int id)
        {
            //Delete degree.
            return "Delete degree: " + id;
        }

        [HttpPost]
        [Route("toggle")]
        public string ToggleCourseToDegree(int id)
        {
            //Toggle course to degree.
            return "Toggle course to degree: " + id;
        }
    }
}
