using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UniShareAPI.Models.DTO.Requests.Comment;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public CommentController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var Comment = await _appDbContext.Comments.AnyAsync(x => x.Id == id);

            if (Comment)
            {
                var CommentRecord = await _appDbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);

                string author = CommentRecord.AuthorId;
                string profile = CommentRecord.ProfileId;

                if (user.Id.Equals(author) || user.Id.Equals(profile))
                {
                    _appDbContext.Comments.Remove(CommentRecord);
                    await _appDbContext.SaveChangesAsync();
                    return Ok("Deleted");
                }
                else
                {
                    return BadRequest("You do not have permission to delete this comment!");

                }
            }
            else
            {
                return BadRequest("No such comment exist!");
            }
        }

        [HttpPost]
        [Route("write")]
        public async Task<IActionResult> PostCommentAsync([FromBody] CommentRequest comment)
        {
            var Writer = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            var Receiver = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == comment.ProfileId);

            string recevier = Receiver.Id;
            string writer = Writer.Id;
            string text = comment.Text;

            Comment commentObj =  new ()
            {
                ProfileId = recevier,
                AuthorId = writer,
                Text = text,
                Date = DateTime.Now
            };

            _appDbContext.Comments.Add(commentObj);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
