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
    public class CourseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        public CourseController(UserManager<IdentityUser> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("statistics")]
        public void GetStatistics()
        {
            //Score
            //Popularity rank
            //Overall ranking
            //Amount of reviews.
        }

        [HttpGet]
        [Route("{id}")]
        public void GetCourse(int id)
        {
            //Get course
        }

        [HttpGet]
        [Route("graph/{id}")]
        public void GetCourseGraphData(int id)
        {
            //Get course
        }

        [HttpPost]
        [Route("set/rate/{id}")]
        public void SetRate([FromBody] RatingRequest rating)
        {
            //Set rating
        }

        [HttpGet]
        [Route("get/rate/{id}")]
        public void GetRating(int id)
        {
            //Get JWT user rating.
        }
    }
}
