using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniShareAPI.Configuration;
using UniShareAPI.Models.DTO;
using UniShareAPI.Models.DTO.Requests;
using UniShareAPI.Models.DTO.Response;
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
        public AuthenticationController(UserManager<User> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParameters,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _tokenValidationParams = tokenValidationParameters;
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                var existingUser = await _userManager.FindByEmailAsync(request.Email);

                if (existingUser != null)
                {
                    return BadRequest( new AuthenticationResult()
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                        },
                    });
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim("Username", user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(30), // 5-10 
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
