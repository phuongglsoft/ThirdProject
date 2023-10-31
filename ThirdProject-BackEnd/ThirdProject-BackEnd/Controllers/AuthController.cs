using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.AuthService;
using ThirdProject_BackEnd.Services.PasswordManager;
using ThirdProject_BackEnd.Services.RefreshTokenService;
using ThirdProject_BackEnd.Services.UserService;

namespace ThirdProject_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IPasswordManager _passwordManager;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IAuthService authService, IUserService userService, IPasswordManager passwordManager, IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _userService = userService;
            _passwordManager = passwordManager;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLoginRequest request)
        {
            var user = await _authService.ValidateUser(request);
            if (user is null)
            {
                return Unauthorized();
            }
            var a = Unauthorized();
            if (!_passwordManager.VerifyPassword(request.password, user.password_hash, user.password_salt))
            {
                return Unauthorized();
            }
            await _authService.UpdateLastLoginTime(user);
            var jwt = _passwordManager.GenerateJWT(user.user_name);
            RefreshToken refreshToken = _refreshTokenService.GenerateRefreshToken(request.user_name);
            await _refreshTokenService.AddRefreshToken(refreshToken);
            UserDTO userDTO = new() { user_name = user.user_name, birth_day = user.birth_day, create_time = user.create_time, last_login = user.last_login };
            AuthResponse response = new() { user = userDTO, jwt = jwt, refresh_token = refreshToken.token };
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] UserRegisterRequest request)
        {
            if (!(await _authService.ValidateUserForRegister(request)))
            {
                return BadRequest("Username is already taken.");
            }
            _passwordManager.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);
            var createTime = DateTime.UtcNow;
            var user = new User
            {
                password_hash = passwordHash,
                password_salt = passwordSalt,
                birth_day = request.birth_day,
                create_time = createTime,
                user_name = request.user_name,
                last_login = createTime,
            };
            await _userService.AddUser(user);
            var jwt = _passwordManager.GenerateJWT(user.user_name);
            RefreshToken refreshToken = _refreshTokenService.GenerateRefreshToken(request.user_name);
            await _refreshTokenService.AddRefreshToken(refreshToken);
            _refreshTokenService.SetFreshToken(refreshToken, Response);
            UserDTO userDTO = new() { user_name = user.user_name, birth_day = user.birth_day, create_time = user.create_time, last_login = user.last_login };
            AuthResponse response = new() { user = userDTO, jwt = jwt, refresh_token = refreshToken.token };
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!await _refreshTokenService.ValidateRefreshToken(request.user_name, request.refresh_token))
            {
                return Unauthorized("Invalid refresh token");
            }
            var newJWT = _passwordManager.GenerateJWT(request.user_name);
            return Ok(newJWT);
        }
    }
}
