using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.PasswordManager;

namespace ThirdProject_BackEnd.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordManager _passwordManager;
        
        public UserService(AppDbContext context, IPasswordManager passwordManager)
        {
            _context = context;
            _passwordManager = passwordManager;
        }

        public async Task<User> AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> ChangePassword(ChangePasswordRequest request)
        {
            var user = await FindByUserName(request.user_name);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            if (request.new_password.Trim().IsNullOrEmpty())
            {
                throw new Exception("Password is empty");
            }
            _passwordManager.CreatePasswordHash(request.new_password, out byte[] passwordHash, out byte[] passwordSalt);
            user.password_hash = passwordHash;
            user.password_salt = passwordSalt;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> FindAll()
        {
            var users = await _context.Users.ToListAsync();
            return users ?? new List<User>();
        }
        public async Task<User?> FindByUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.user_name == userName);
     
        }
        public async Task<User> ChangeBirthDay(ChangeBirthdayRequest request)
        {
            var user = await FindByUserName(request.user_name);
            if(user is null)
            {
                throw new Exception("User not found");
            }
            user.birth_day = request.new_birth_day;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
