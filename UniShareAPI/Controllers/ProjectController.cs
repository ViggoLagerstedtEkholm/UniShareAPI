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
    public class ProjectController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public ProjectController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public string Project(int id)
        {
            return "Get project: " + id;
        }

        [HttpPost]
        [Route("delete/{id}")]
        public string Delete(int id)
        {
            //Check if user own is before delete.
            return "Delete project: " + id;
        }

        [HttpPost]
        [Route("edit/{id}")]
        public string Update(int id)
        {
            //Update project.
            return "Update project: " + id;
        }

        [HttpPost]
        [Route("upload")]
        public string Upload(int id)
        {
            //Upload project.
            return "Upload project: " + id;
        }
    }
}
