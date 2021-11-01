using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Requests.Course;
using UniShareAPI.Models.DTO.Response.Courses;
using UniShareAPI.Models.DTO.Response.Search.Courses;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public CourseController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseUpdateRequest course)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var existingCourse = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id == course.CourseID);

            existingCourse.Id = course.CourseID;
            existingCourse.City = course.City;
            existingCourse.Code = course.Code;
            existingCourse.Country = course.Country;
            existingCourse.Credits = course.Credits;
            existingCourse.Description = course.Description;
            existingCourse.Link = course.Link;
            existingCourse.Name = course.Name;
            existingCourse.University = course.University;           

            _appDbContext.Courses.Update(existingCourse);
            await _appDbContext.SaveChangesAsync();

            return Ok("Uploaded request");
        }

        [HttpGet]
        [Route("statistics/{id}")]
        public async Task<IActionResult> GetCourseStatisticsAsync(int id)
        {
            var course = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(id));

            if (course != null)
            {
                double RatingAverage = _appDbContext.UserCourses.Where(r => r.CourseId == id).Average(r => r.Rating);
                int count = _appDbContext.UserCourses.Where(r => r.CourseId == id).Count();

                var statistics = new Statistics()
                {
                    Count = count,
                    Rating = RatingAverage
                };

                return Ok(statistics);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCourseAsync(int id)
        {
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return Ok(course);
        }


        [HttpGet]
        [Route("graph/{id}")]
        public IActionResult GetGraphData(int id)
        {
            var results = (from item in _appDbContext.UserCourses
                           where item.CourseId == id
                           group item by new { item.Rating} into grouping
                           orderby grouping.Key.Rating descending
                           select new GraphData
                           {
                              Rating = grouping.Key.Rating,
                              Count = grouping.Count()
                           }).ToList();


            return Ok(results);
        }

        [HttpPost]
        [Route("set/rating")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetCourseRating([FromBody] Rating rating)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(rating.Id));

            string userId = user.Id;
            int courseId = course.Id;

            if (rating.Score == 0)
            {
                var removeRating = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.User.Id.Equals(userId) && x.CourseId == course.Id);
                _appDbContext.Remove(removeRating);
                _appDbContext.SaveChanges();
                return Ok();
            }

            if(course == null)
            {
                return NotFound();
            }


            var existingRating = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.User.Id.Equals(userId) && x.CourseId == course.Id);

            if(existingRating == null)
            {
                UserCourse userCourse = new()
                {
                    User = user,
                    Course = course,
                    Rating = rating.Score
                };

                await _appDbContext.UserCourses.AddAsync(userCourse);
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                existingRating.Rating = rating.Score;
                _appDbContext.UserCourses.Update(existingRating);
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpGet]
        [Route("get/rating/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRating(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));

            var ratingRecord = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(course.Id) && x.UserId.Equals(user.Id));

            if(ratingRecord == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(ratingRecord.Rating);
            }
        }
    }
}
