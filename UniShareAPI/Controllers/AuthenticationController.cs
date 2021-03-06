using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UniShareAPI.Configuration;
using UniShareAPI.Models.DTO;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Requests.Auth;
using UniShareAPI.Models.DTO.Response;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")] // api/authManagement
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(UserManager<User> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            AppDbContext appDbContext,
            ILogger<AuthenticationController> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParameters;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("Delete/Account")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = HttpContext.GetUserId();

            var user = await _userManager.FindByIdAsync(userId);

            var relation = _appDbContext.Relations.Where(x => x.FromId.Equals(userId) || x.ToId.Equals(userId));
            _appDbContext.Relations.RemoveRange(relation);

            var comments = _appDbContext.Comments.Where(x => x.AuthorId.Equals(userId) || x.ProfileId.Equals(userId));
            _appDbContext.Comments.RemoveRange(comments);

            await _appDbContext.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("Removed user.");
            }

            return BadRequest("Could not remove user.");
        }

        [HttpPost]
        [Route("Verify")]
        public async Task<IActionResult> VerifyAsync([FromBody] VerifyRequest verifyRequest)
        {
            if(verifyRequest.Id == null || verifyRequest.Token == null)
            {
                return BadRequest("Invalid verification. 1");
            }

            var user = await _userManager.FindByIdAsync(verifyRequest.Id);

            if (user == null)
            {
                return BadRequest("No such user.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, verifyRequest.Token);

            if (result.Succeeded)
            {
                return Ok("Verified email!");
            }
           
           return BadRequest("Invalid verification. Id: " + verifyRequest.Id + ", Token: " + verifyRequest.Token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUserWithEmail = await _userManager.FindByEmailAsync(request.Email);

                if (existingUserWithEmail != null)
                {
                    return BadRequest("Email already in use");
                }

                var existingUserWithUsername = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(request.Username));

                if (existingUserWithUsername != null)
                {
                    return BadRequest("Username already in use");
                }

                var newUser = new User() {
                    Email = request.Email,
                    UserName = request.Username,
                    Firstname = request.Firstname,
                    Lastname = request.Lastname,
                    Age = request.Age,
                    Joined = DateTime.Now,
                };

                var isCreated = await _userManager.CreateAsync(newUser, request.Password);

                if (isCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "Standard");

                    /*
                    var success = await SendVerificationEmailAsync(newUser);

                    if (!success)
                    {
                        await _userManager.DeleteAsync(newUser);
                        return BadRequest("Could not send verification email!" );
                    }
                    */
                }

                if (!isCreated.Succeeded)
                {
                    return BadRequest(new AuthenticationResult
                    {
                        Errors = isCreated.Errors.Select(x => x.Description)
                    });
                }

                var res = await GenerateAuthenticationResultForUserAsync(newUser);

                return Ok(res);
            }

            return BadRequest(new AuthenticationResult()
            {
                Errors = new List<string>() {
                    "Invalid payload"
                },
            });
        }

        private async Task<bool> SendVerificationEmailAsync(User RegisteredUser)
        {
            var success = true;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(RegisteredUser);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = "http://localhost:3000/Verify?Id=" + RegisteredUser.Id + "&Token=" + encodedToken;

            SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "EMAIL",
                    Password = "PASSWORD"
                }
            };

            MailAddress FromEmail = new MailAddress("unishare.validation@gmail.com", "UniShare");
            MailAddress ToEmail = new MailAddress(RegisteredUser.Email, RegisteredUser.UserName);
            MailMessage Message = new MailMessage()
            {
                From = FromEmail,
                Subject = "Verification",
                Body = "Here is your verification URL: " + confirmationLink
            };

            Message.To.Add(ToEmail);
            try
            {
                smtpClient.Send(Message);
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new AuthenticationResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var res = await GenerateAuthenticationResultForUserAsync(existingUser);

                //Add last seen date on login.
                existingUser.LastSeenDate = DateTime.Now;
                await _userManager.UpdateAsync(existingUser);

                return Ok(res);
            }

            return BadRequest(new AuthenticationResult()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var claims = await GetAllValidClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Used = false,
                IsRevoked = false,
                Token = RandomString(35) + Guid.NewGuid()
            };

            await _appDbContext.RefreshTokens.AddAsync(refreshToken);
            await _appDbContext.SaveChangesAsync();

            return new AuthenticationResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }

        private async Task<List<Claim>> GetAllValidClaims(User user)
        {
            var options = new IdentityOptions();

            var claims = new List<Claim>
            {
                    new Claim("Id", user.Id),
                    new Claim("Username", user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Getting the claims that we have assigned to the user
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Get the user role and add it to the claims
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);

                if (role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest tokenRequest)
        {
            var validatedToken = GetPrincipalFromToken(tokenRequest.Token);

            if(validatedToken == null)
            {
                return BadRequest(new AuthenticationResult{Errors = new[] { "Invalid Token" }});
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(X => X.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if(expiryDateTimeUtc > DateTime.UtcNow)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Token has not yet expired!" } });
            }

            var jti = validatedToken.Claims.Single(X => X.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _appDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if(storedRefreshToken == null)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Refresh token does not exist" } });
            }

            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Refresh token has expired" } });
            }

            if (storedRefreshToken.IsRevoked)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Refresh token has been invalidated" } });
            }

            if (storedRefreshToken.Used)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Refresh token has been used" } });
            }

            if(storedRefreshToken.JwtId != jti)
            {
                return BadRequest(new AuthenticationResult { Errors = new[] { "Refresh token does not match this JWT" } });
            }

            storedRefreshToken.Used = true;
            _appDbContext.RefreshTokens.Update(storedRefreshToken);
            await _appDbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "Id").Value);
            var res = await GenerateAuthenticationResultForUserAsync(user);
            return Ok(res);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _tokenValidationParams.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;
            }catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && 
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
