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
    public class ReviewController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public ReviewController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet("{id}")]
        public string Get(int Id)
        {
            return "Get review.";
        }

        [HttpPost]
        [Route("delete")]
        public string Delete(int id)
        {
            //Upload project.
            return "Delete review: " + id;
        }

        [HttpPost]
        [Route("upload")]
        public string Upload(int id)
        {
            //Upload project.
            return "Upload review: " + id;
        }
    }
}
