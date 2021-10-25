using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Course;
using UniShareAPI.Models.DTO.Requests.Review;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;
using static System.Net.Mime.MediaTypeNames;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
    public class ReviewController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public ReviewController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("write")]
        public async Task<IActionResult> PostReview([FromBody] ReviewRequest reviewRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            var existingCourse = await _appDbContext.Reviews.AnyAsync(x => x.CourseId.Equals(reviewRequest.CourseId) && x.UserId.Equals(userId));

            if (existingCourse)
            {
                var course = await _appDbContext.Reviews.FirstOrDefaultAsync(x => x.CourseId.Equals(reviewRequest.CourseId) && x.UserId.Equals(userId));

                course.CourseId = reviewRequest.CourseId;
                course.UserId = userId;
                course.Difficulty = reviewRequest.Difficulty;
                course.Environment = reviewRequest.Environment;
                course.Fulfilling = reviewRequest.Fulfilling;
                course.Grading = reviewRequest.Grading;
                course.Literature = reviewRequest.Literature;
                course.Overall = reviewRequest.Overall;
                course.Text = reviewRequest.Text;
                course.UserId = userId;
                course.UpdatedDate = DateTime.UtcNow.Date;

                _appDbContext.Update(course);
                await _appDbContext.SaveChangesAsync();

                return Ok();
            }
            else
            {
                var review = new Review()
                {
                    Difficulty = reviewRequest.Difficulty,
                    Environment = reviewRequest.Environment,
                    Fulfilling = reviewRequest.Fulfilling,
                    Grading = reviewRequest.Grading,
                    Literature = reviewRequest.Literature,
                    Overall = reviewRequest.Overall,
                    Text = reviewRequest.Text,
                    UserId = userId,
                    CourseId = reviewRequest.CourseId,
                    AddedDate = DateTime.UtcNow.Date,
                    UpdatedDate = DateTime.UtcNow.Date
                };

                _appDbContext.Add(review);
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _appDbContext.Reviews.FirstOrDefaultAsync(x => x.UserId.Equals(HttpContext.GetUserId()) && x.CourseId.Equals(id));

            if(review == null)
            {
                return BadRequest("Can't delete that review!");
            }

            _appDbContext.Remove(review);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var review = await _appDbContext.Reviews.FirstOrDefaultAsync(x => x.UserId.Equals(HttpContext.GetUserId()) && x.CourseId.Equals(id));
            return Ok(review);
        }
    }
}
