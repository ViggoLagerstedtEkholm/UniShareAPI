using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Requests.Course;
using UniShareAPI.Models.DTO.Requests.Degree;
using UniShareAPI.Models.DTO.Response.Degrees;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
    public class DegreeController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public DegreeController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadDegree([FromForm] DegreeUploadRequest degreeUpload)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var degree = new Degree
            {
                Name = degreeUpload.Name,
                City = degreeUpload.City,
                StartDate = degreeUpload.StartDate,
                EndDate = degreeUpload.EndDate,
                University = degreeUpload.University,
                Field = degreeUpload.Field,
                Country = degreeUpload.Country,
                User = user
            };

            await _appDbContext.Degrees.AddAsync(degree);
            await _appDbContext.SaveChangesAsync();

            user.ActiveDegreeId = degree.Id;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPost]
        [Route("toggle/{id}")]
        public async Task<IActionResult> ToggleActiveDegree(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id.Equals(id));

            string userId = user.Id;
            int courseId = course.Id;

            int? activeDegree = user.ActiveDegreeId;
            if(activeDegree == null)
            {
                return BadRequest("You need to register an active degree!");
            }

            var courseInDegree = await _appDbContext.DegreeCourses.FirstOrDefaultAsync(x => x.DegreeId.Equals(activeDegree) && x.CourseId == id);
            
            DegreeCourse degreeCourse = new ()
            {
                CourseId = id,
                DegreeId = (int) activeDegree,
            };

            if (courseInDegree != null){
                //Remove from degree.
                _appDbContext.DegreeCourses.Remove(courseInDegree);
                await _appDbContext.SaveChangesAsync();
                return Ok(false);
            }
            else
            {
                //Add to degree.
                await _appDbContext.DegreeCourses.AddAsync(degreeCourse);
                await _appDbContext.SaveChangesAsync();
                return Ok(true);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateDegree([FromForm] DegreeUpdateRequest degreeUpdate)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var degree = await _appDbContext.Degrees.FirstOrDefaultAsync(x => x.Id == degreeUpdate.Id);

            degree.Name = degreeUpdate.Name;
            degree.City = degreeUpdate.City;
            degree.StartDate = degreeUpdate.StartDate;
            degree.EndDate = degreeUpdate.EndDate;
            degree.University = degreeUpdate.University;
            degree.Country = degreeUpdate.Country;
            degree.Field = degreeUpdate.Field;

            _appDbContext.Degrees.Update(degree);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("user/{username}")]
        [AllowAnonymous]
        public IActionResult GetDegrees(string username)
        {
            var degrees = _appDbContext.Degrees.Where(x => x.User.UserName.Equals(username));

            if (degrees == null)
            {
                return NotFound();
            }

            List<DegreeWithCourses> degreeWithCourses = new List<DegreeWithCourses>();

            foreach(Degree aDegree in degrees.ToList())
            {
                int degreeId = aDegree.Id;

                var result = (
                from courses in _appDbContext.Courses
                join degreeCourses in _appDbContext.DegreeCourses
                on courses.Id equals degreeCourses.CourseId
                where degreeCourses.DegreeId == degreeId
                select courses).ToList();

                double totalCredits = 0;
                foreach(var course in result)
                {
                    totalCredits += course.Credits;
                }

                DegreeWithCourses coursesDegree = new()
                {
                    Courses = result,
                    Degree = aDegree,
                    TotalCredits = totalCredits
                };

                degreeWithCourses.Add(coursesDegree);
            }
            return Ok(degreeWithCourses);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDegree(int id)
        {
            var degree = await _appDbContext.Degrees.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return Ok(degree);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteDegree(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var degreeToDelete = await _appDbContext.Degrees.FirstOrDefaultAsync(x => x.Id == id);

            if (degreeToDelete.User.Id == user.Id)
            {
                _appDbContext.Remove(degreeToDelete);
                _appDbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("You do not own this degree!");
            }
        }
    }
}
