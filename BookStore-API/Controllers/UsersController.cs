using BookStore_API.DTOs.Log;
using BookStore_API.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILoggerService _logger;
        private readonly IConfiguration _config;

        public UsersController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              ILoggerService logger,
                              IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LogIn([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionName();
            try
            {
                var userName = userDTO.UserName;
                var userPasword = userDTO.Password;

                _logger.LogInfo($"{location} : {userName} - Attempting to Login");
                var result = await _signInManager.PasswordSignInAsync(userName, userPasword, false, false);

                if (result.Succeeded)
                {
                    _logger.LogInfo($"{location} - Logged Successful");
                    var user = await _userManager.FindByNameAsync(userName);
                    var stringToken = GenerateJWT(user);
                    return Ok(stringToken);
                }

                _logger.LogInfo($"{location} : {userName} - Unauthorized");
                return Unauthorized(userDTO);
            }

            catch (Exception e)
            {
                return InternalError(e, location);
            }
        }

        private async Task<string> GenerateJWT(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            /*
                user.id
                user.UserName
                GUID
                user roles

            */
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultNameClaimType, r)));
    
            /*
            string issuer = null, 
            string audience = null, 
            IEnumerable< Claim > claims = null, 
            DateTime? notBefore = null, 
            DateTime? expires = null, 
            SigningCredentials signingCredentials = null

            */

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                null,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials : credentials
                ); 
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
        

        private string GetControllerActionName()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return ($"{controller} - {action}");
        }

        private ObjectResult InternalError(Exception e, string location)
        {
            _logger.LogError($"{location}: {e.Message} - {e.InnerException}");
            return StatusCode(500, "Something went wrong, contact the administrator");
        }

    }
}
