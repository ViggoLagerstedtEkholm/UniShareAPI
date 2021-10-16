using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Requests.Project;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;

        public ProjectController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadProject([FromForm] ProjectUpload projectUpload)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
           
            try
            {
                using var stream = new MemoryStream();
                projectUpload.File.CopyTo(stream);
                var fileBytes = stream.ToArray();

                var project = new Project
                {
                    Image = fileBytes,
                    UserId = user.Id,
                    Link = projectUpload.Link,
                    Description = projectUpload.Description,
                    Name = projectUpload.Name,
                    Added = DateTime.Now
                };

                await _appDbContext.Projects.AddAsync(project);
                await _appDbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateProject([FromForm] ProjectUpdate projectUpdate)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var projectToUpdate = await _appDbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectUpdate.Id);

            if (projectToUpdate != null && projectToUpdate.UserId.Equals(user.Id))
            {
                try
                {
                    using var stream = new MemoryStream();
                    projectUpdate.File.CopyTo(stream);
                    var fileBytes = stream.ToArray();

                    var project = await _appDbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectUpdate.Id);
                    project.Image = fileBytes;
                    project.Name = projectUpdate.Name;
                    project.Description = projectUpdate.Description;
                    project.Link = projectUpdate.Link;

                    _appDbContext.Projects.Update(project);
                    await _appDbContext.SaveChangesAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("You do not own this project or it does not exist!" + projectUpdate.Id);
            }

        }

        [HttpGet]
        [Route("user/{username}")]
        [AllowAnonymous]
        public IActionResult GetProjects(string username)
        {
            var projects = _appDbContext.Projects.Where(x => x.User.UserName.Equals(username));
            return Ok(projects);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProjectAsync(int id)
        {
            var project = await _appDbContext.Projects.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return Ok(project);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());

            var projectToDelete = await _appDbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if(projectToDelete.UserId == user.Id)
            {
                _appDbContext.Remove(projectToDelete);
                _appDbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("You do not own this project!");
            }
        }
    }
}
