using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.PasswordManager;

namespace ThirdProject_BackEnd.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordManager _passwordManager;
        public AuthService(AppDbContext context, IConfiguration configuration, IPasswordManager passwordManager)
        {
            _context = context;
            _passwordManager = passwordManager;
        }


        public async Task UpdateLastLoginTime(User user)
        {
            user.last_login = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<User?> ValidateUser(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.user_name == request.user_name);
            if (user is null)
            {
                return default;
            }

            if (!_passwordManager.VerifyPassword(request.password, user.password_hash, user.password_salt))
            {
                return default;
            }

            return user;
        }
        public async Task<bool> ValidateUserForRegister(UserRegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.user_name == request.user_name))
            {
                return false;
            }
            return true;
        }

    }
}
