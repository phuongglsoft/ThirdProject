using ThirdProject_BackEnd.Models;

namespace ThirdProject_BackEnd.Services.AuthService
{
    public interface IAuthService
    {
        Task<User?> ValidateUser(UserLoginRequest request);
        Task<bool> ValidateUserForRegister(UserRegisterRequest request);
        Task UpdateLastLoginTime(User user);
    }
    
}
