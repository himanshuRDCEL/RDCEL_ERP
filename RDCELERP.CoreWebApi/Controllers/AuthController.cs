using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RDCELERP.DAL.Entities;
using RDCELERP.Model;
using RDCELERP.Model.Users;
using RDCELERP.CoreWebApi.Configuration;
using RDCELERP.CoreWebApi.Models;
using RDCELERP.DAL.Repository;

namespace RDCELERP.CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtBearerTokenSettings jwtBearerTokenSettings;
        private readonly UserManager<IdentityUser> userManager;
        private readonly Digi2l_DevContext _context;
        //LoginRepository _loginRepository ;
        public AuthController(IOptions<JwtBearerTokenSettings> jwtTokenOptions, Digi2l_DevContext context)
        {
            this.jwtBearerTokenSettings = jwtTokenOptions.Value;
            _context = context;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDetails userDetails)
        {
            if (!ModelState.IsValid || userDetails == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            var identityUser = new IdentityUser() { UserName = userDetails.UserName, Email = userDetails.Email };
            var result = await userManager.CreateAsync(identityUser, userDetails.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }

            return Ok(new { Message = "User Reigstration Successful" });
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Login([FromForm] LoginCredentials credentials)
        {
            IdentityUser identityUser;
            LoginModel loginModel = new LoginModel();
            if (!ModelState.IsValid
                || credentials == null)
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

            //TblLoginMobile user = _context.TblLoginMobiles.FirstOrDefault(u => u.Username.ToLower().Equals(credentials.Username.ToLower()) && u.Password.Equals(credentials.Password));
            Login user = _context.Logins.FirstOrDefault(x => x.Username.ToLower().Equals(credentials.Username.ToLower()) && x.Password.Equals(credentials.Password));

            if (user != null && user.Id > 0)
            {
                loginModel.Username = user.Username;
                loginModel.Password = user.Password;
                loginModel.email = user.Username;
                var token = GenerateToken(loginModel);
                return Ok(new { Token = token, Message = "Success" });
            }
            else
            {
                return new BadRequestObjectResult(new { Message = "Login failed" });
            }

        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Well, What do you want to do here ?
            // Wait for token to get expired OR 
            // Maintain token cache and invalidate the tokens after logout method is called
            return Ok(new { Token = "", Message = "Logged Out" });
        }

        private async Task<IdentityUser> ValidateUser(LoginCredentials credentials)
        {
            //var identityUser = await userManager.FindByNameAsync(credentials.Username);

            //_context.Logins.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(email.ToLower()) && u.Password.Equals(password));
            //var identityUser = _context.Logins.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(email.ToLower()) && u.Password.Equals(password));

            //if (identityUser != null)
            //{
            //    var result = userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
            //    return result == PasswordVerificationResult.Failed ? null : identityUser;
            //}

            return null;
        }


        private object GenerateToken(LoginModel identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identityUser.Username.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Username)
                }),

                Expires = DateTime.UtcNow.AddSeconds(jwtBearerTokenSettings.ExpiryTimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtBearerTokenSettings.Audience,
                Issuer = jwtBearerTokenSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}