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
    public class ForumController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public ForumController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            //Get forum.
            return "Get forum: " + id;
        }

        [HttpGet]
        [Route("Add")]
        public string Add(int id)
        {
            //Add forum.
            return "Add forum: " + id;
        }
    }
}
