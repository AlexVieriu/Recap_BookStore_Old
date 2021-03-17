using BookStore_API.DTOs.Log;
using BookStore_API.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public UsersController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              ILoggerService logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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

                if(result.Succeeded)
                {
                    _logger.LogInfo($"{location} - Logged Successful");
                    var user = await _userManager.FindByNameAsync(userName);
                    return Ok(user);
                }

                _logger.LogInfo($"{location} : {userName} - Unauthorized");
                return Unauthorized(userDTO);
            }

            catch (Exception e)
            {
                return InternalError(e, location);
            }


           
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
