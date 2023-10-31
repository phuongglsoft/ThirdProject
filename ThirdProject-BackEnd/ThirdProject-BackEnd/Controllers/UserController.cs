using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.UserService;

namespace ThirdProject_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("{id}/change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity is null)
            {
                return Unauthorized("Invalid token");
            }
            var userCLaims = identity.Claims;
            var userName = userCLaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            if(userName is null)
            {
                return Unauthorized("Invalid token");
            }
            if(request.user_name != userName)
            {
                return Unauthorized("Invalid token");
            }
            await _userService.ChangePassword(request);
            return NoContent();
        }

        [HttpPatch("{id}/change-birthday")]
        [Authorize]
        public async Task<ActionResult> ChangeBirthDay([FromBody]ChangeBirthdayRequest request ) {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null)
            {
                return Unauthorized("Invalid token");
            }
            var userCLaims = identity.Claims;
            var userName = userCLaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userName is null)
            {
                return Unauthorized("Invalid token");
            }
            if (request.user_name != userName)
            {
                return Unauthorized("Invalid token");
            }
            await _userService.ChangeBirthDay(request);
            return NoContent();
        }
    }
}
