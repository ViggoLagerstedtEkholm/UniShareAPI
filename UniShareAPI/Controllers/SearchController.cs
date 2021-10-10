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
    public class SearchController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public SearchController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("people")]
        public string People()
        {
            return "Search people.";
        }

        [HttpGet]
        [Route("courses")]
        public string Courses()
        {
            return "Search courses.";
        }

        [HttpGet]
        [Route("forums")]
        public string Forums()
        {
            return "Search forums.";
        }

        [HttpGet]
        [Route("posts")]
        public string Posts()
        {
            return "Search posts.";
        }

        [HttpGet]
        [Route("reviews")]
        public string Reviews()
        {
            return "Search reviews.";
        }

        [HttpGet]
        [Route("user/reviews")]
        public string UserReviews()
        {
            return "Search user reviews.";
        }

        [HttpGet]
        [Route("user/ratings")]
        public string UserRatings()
        {
            return "Search user ratings.";
        }

        [HttpGet]
        [Route("requests")]
        public string Requests()
        {
            return "Search requests.";
        }

        [HttpGet]
        [Route("comments")]
        public string Comments()
        {
            return "Search comments.";
        }
    }
}
