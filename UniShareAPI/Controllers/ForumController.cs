using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public ForumController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

    }
}
