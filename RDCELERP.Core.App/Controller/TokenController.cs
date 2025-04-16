using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RDCELERP.Model.Base;

namespace RDCELERP.Core.App.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IOptions<ApplicationSettings> _config;

        public TokenController(IOptions<ApplicationSettings> config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult GenerateToken([FromBody] LoginModel login)
        {
            if (login.Username ==_config.Value.JWTUserName && login.Password == _config.Value.JWTPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config.Value.JWTKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login.Username) }),
                    Expires = DateTime.UtcNow.AddHours(1), // Token valid for 1 hour
                    Issuer = _config.Value.JWTIssuer,
                    Audience = _config.Value.JWTAudience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized();
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
