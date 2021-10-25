using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Admin")]
    public class RequestController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public RequestController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadRequest([FromBody] CourseRequest courseRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var request = new Request()
            {
                City = courseRequest.City,
                Code = courseRequest.Code,
                Country = courseRequest.Country,
                Credits = courseRequest.Credits,
                Date = DateTime.Now,
                Description = courseRequest.Description,
                Link = courseRequest.Link,
                Name = courseRequest.Name,
                University = courseRequest.University,
                UserId = user.Id
            };

            await _appDbContext.Requests.AddAsync(request);
            await _appDbContext.SaveChangesAsync();

            return Ok("Uploaded request");
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateRequest([FromBody] CourseRequest courseRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var request = new Request()
            {
                City = courseRequest.City,
                Code = courseRequest.Code,
                Country = courseRequest.Country,
                Credits = courseRequest.Credits,
                Date = DateTime.Now,
                Description = courseRequest.Description,
                Link = courseRequest.Link,
                Name = courseRequest.Name,
                University = courseRequest.University,
                UserId = user.Id
            };

            await _appDbContext.Requests.AddAsync(request);
            await _appDbContext.SaveChangesAsync();

            return Ok("Uploaded request");
        }


        [HttpPost]
        [Route("delete/{requestId}")]
        public async Task<IActionResult> DeleteRequest(int requestId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var request = await _appDbContext.Requests.FirstOrDefaultAsync(x => x.Id.Equals(requestId));

            if (!request.UserId.Equals(user.Id))
            {
                return BadRequest("You can't delete this request!");
            }

            _appDbContext.Requests.Remove(request);
            _appDbContext.SaveChanges();

            return Ok("Removed request");
        }


        [HttpGet]
        [Route("all")]
        public IActionResult Requests()
        {
            var requests = _appDbContext.Requests.Where(x => x.UserId.Equals(HttpContext.GetUserId())).ToList();

            return Ok(requests);
        }

        [HttpPost]
        [Route("approve/{requestId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _appDbContext.Requests.FirstOrDefaultAsync(x => x.Id.Equals(requestId));

            if(request == null)
            {
                return BadRequest("No such request exists!");
            }

            var course = new Course()
            {
                Country = request.Country,
                Code = request.Code,
                City = request.City,
                Credits = request.Credits,
                Description = request.Description,
                Link = request.Link,
                Name = request.Name,
                University = request.University,
                Added = DateTime.Now,
            };

            using var transaction = _appDbContext.Database.BeginTransaction();

            try
            {
                _appDbContext.Requests.Remove(request);
                _appDbContext.SaveChanges();

                _appDbContext.Courses.Add(course);
                _appDbContext.SaveChanges();

                transaction.Commit();
            }
            catch(Exception ex)
            {
                return BadRequest("COULD NOT HANDLE APPROVED REQUEST! Exception: " + ex.Message);
            }

            return Ok("Removed request");
        }

        [HttpPost]
        [Route("deny/{requestId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DenyRequest(int requestId)
        {
            var request = await _appDbContext.Requests.FirstOrDefaultAsync(x => x.Id.Equals(requestId));

            if (request == null)
            {
                return BadRequest("No such request exists!");
            }

            _appDbContext.Requests.Remove(request);
            _appDbContext.SaveChanges();

            return Ok("Removed request");
        }
    }
}
