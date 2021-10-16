using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Response.Home;
using UniShareAPI.Models.DTO.Response.Search.Courses;
using UniShareAPI.Models.Relations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public HomeController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("statistics")]
        public async Task<IActionResult> StatisticsAsync()
        {
            var peopleCount = _appDbContext.Users.Count();
            var coursesCount = _appDbContext.Courses.Count();

            var topCourses = _appDbContext.Courses
              .OrderByDescending(m => m.Ratings.Average(r => r.Rating))
              .Take(10).ToList();

            var courses = _appDbContext.Courses.Select(p => new CourseResponse
                            {
                                Name = p.Name,
                                University = p.University,
                                Added = p.Added,
                                City = p.City,
                                Code = p.Code,
                                Country = p.Country,
                                Credits = p.Credits,
                                Link = p.Link,
                                Id = p.Id,
                                Rating = 0,
                                InActiveDegree = false
                            }).ToList();

            //Find rating for all courses. Might have to rework later for performance...
            foreach (CourseResponse courseResponse in courses)
            {
                var course = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(courseResponse.Id));

                if (course != null)
                {
                    courseResponse.Rating = _appDbContext.UserCourses.Where(r => r.CourseId == courseResponse.Id).Average(x => x.Rating);
                }
            }

            var statistics = new Statistics()
            {
                People = peopleCount,
                Courses = coursesCount,
                TopCourses = courses.OrderByDescending(x => x.Rating).Take(10).ToList()
            };

            return Ok(statistics);
        }
    }
}
