using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Course;
using UniShareAPI.Models.DTO.Response.Courses;
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
        public async Task<IActionResult> GetGraphData(int id)
        {
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return Ok(course);
        }

        [HttpPost]
        [Route("set/rating")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetCourseRating([FromBody] Rating rating)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(rating.Id));

            if(course == null)
            {
                return NotFound();
            }

            string userId = user.Id;
            int courseId = course.Id;

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
