using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        public FriendsController(UserManager<User> userManager,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        //TODO
    }
}
