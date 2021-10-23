using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UniShareAPI.Models.DTO.Response.Friends;
using UniShareAPI.Models.DTO.Response.Search.Comments;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FriendsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public FriendsController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        //Send request
        [HttpPost]
        [Route("request/{otherId}")]
        public async Task<IActionResult> SendRequest(string otherId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            //Self request
            if (userId.Equals(otherId))
            {
                return BadRequest("You can't send friend request to yourself!");
            }

            //Check if friends.
            var isFriend = await _appDbContext.Relations.AnyAsync(
                                                    x => x.FromId.Equals(userId) &&
                                                    x.ToId.Equals(otherId) && x.Status.Equals("F"));
            if(isFriend)
            {
                return BadRequest("Already friends!");
            }

            //Check if in pending request
            var isAlreadyInPending = await _appDbContext.Relations.AnyAsync(
                                        x => x.Status.Equals("P") && x.FromId.Equals(userId) && x.ToId.Equals(otherId) ||
                                        x.Status.Equals("P") && x.FromId.Equals(otherId) && x.ToId.Equals(userId));

            if(isAlreadyInPending)
            {
                return BadRequest("Already a pending request!");
            }

            var relation = new Relation()
            {
                FromId = userId,
                ToId = otherId,
                Status = "P",
                Since = DateTime.Now
            };

            await _appDbContext.Relations.AddAsync(relation);
            await _appDbContext.SaveChangesAsync();

            return Ok("Sent request!");
        }

        [HttpPost]
        [Route("accept/{otherId}")]
        public async Task<IActionResult> AcceptRequest(string otherId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            var relation = await _appDbContext.Relations.FirstOrDefaultAsync(x => x.FromId.Equals(otherId) && x.ToId.Equals(userId));
            relation.Status = "F";
            _appDbContext.Update(relation);
            _appDbContext.SaveChanges();

            var receivedRelation = new Relation()
            {
                FromId = userId,
                ToId = otherId,
                Status = "F",
                Since = DateTime.Now
            };

            await _appDbContext.Relations.AddAsync(receivedRelation);
            await _appDbContext.SaveChangesAsync();

            return Ok("Do something...");
        }

        [HttpPost]
        [Route("cancel/{otherId}")]
        public async Task<IActionResult> CancelRequest(string otherId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            Relation relation = await _appDbContext.Relations.FirstOrDefaultAsync(x => x.FromId.Equals(userId) && x.ToId.Equals(otherId));

            _appDbContext.Remove(relation);
            _appDbContext.SaveChanges();

            return Ok("Canceled request.");
        }

        [HttpPost]
        [Route("unfriend/{otherId}")]
        public async Task<IActionResult> Unfriend(string otherId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;
            var relation = _appDbContext.Relations.Where(x =>
            x.FromId.Equals(userId) && x.ToId.Equals(otherId) ||
            x.FromId.Equals(otherId) && x.ToId.Equals(userId));

            _appDbContext.RemoveRange(relation);
            _appDbContext.SaveChanges();

            return Ok("Unfriended.");
        }

        [HttpPost]
        [Route("block/{otherId}/{block}")]
        public async Task<IActionResult> Block(string otherId, bool blocked = true)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            if (blocked)
            {
                var relation = new Relation()
                {
                    FromId = userId,
                    ToId = otherId,
                    Status = "B",
                    Since = DateTime.Now
                };

               await _appDbContext.AddAsync(relation);
               await _appDbContext.SaveChangesAsync();
            }
            else
            {
                var relation = await _appDbContext.Relations.FirstOrDefaultAsync(x => x.FromId.Equals(userId) && x.ToId.Equals(otherId));
                _appDbContext.Remove(relation);
                _appDbContext.SaveChanges();
            }

            return BadRequest("Do something...");
        }

        [HttpGet]
        [Route("pending/received")]
        public async Task<IActionResult> GetPendingReceivedRequests()
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == HttpContext.GetUserId());
            string userId = user.Id;

            //Get incoming friend request ( from people to user )
            var outgoing = _appDbContext.Relations
                          .Join(_appDbContext.User, u => u.FromId, uir => uir.Id, (u, uir) => new { u, uir })
                          .Where(m => m.u.ToId.Equals(userId) && m.u.Status.Equals("P"))
                          .Select(p => new SentRequest
                          {
                              Image = p.uir.Image,
                              UserId = p.uir.Id,
                              Username = p.uir.UserName,
                          });

            return Ok(outgoing);
        }

        [HttpGet]
        [Route("pending/sent")]
        public IActionResult GetPendingSentRequests()
        {
            string userId = HttpContext.GetUserId();

            var outgoing = _appDbContext.Relations
                 .Join(_appDbContext.User, u => u.ToId, uir => uir.Id, (u, uir) => new { u, uir })
                 .Where(m => m.u.FromId.Equals(userId) && m.u.Status.Equals("P"))
                 .Select(p => new SentRequest
                 {
                    Image = p.uir.Image,
                    UserId = p.uir.Id,
                    Username = p.uir.UserName,
                 });

            return Ok(outgoing);
        }

        [HttpGet]
        [Route("friends/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFriendsAsync(string username)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(username));
            string userId = user.Id;

            var friends = _appDbContext.User
                          .Join(_appDbContext.Relations, u => u.Id, uir => uir.ToId, (u, uir) => new { u, uir })
                          .Where(m => m.uir.FromId.Equals(userId) && m.uir.Status.Equals("F"))
                          .Select(p => new SentRequest
                          {
                              Image = p.u.Image,
                              UserId = p.u.Id,
                              Username = p.u.UserName,
                          });

            return Ok(friends);
        }
    }
}
