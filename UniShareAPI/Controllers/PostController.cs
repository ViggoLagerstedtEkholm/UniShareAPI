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
    public class PostController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public PostController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("upload")]
        public string Post(int id)
        {
            //Upload post.
            return "Upload post: " + id;
        }
    }
}
