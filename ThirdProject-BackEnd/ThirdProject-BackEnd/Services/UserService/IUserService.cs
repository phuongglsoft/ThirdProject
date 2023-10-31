using ThirdProject_BackEnd.Models;

namespace ThirdProject_BackEnd.Services.UserService
{
    public interface IUserService
    {
        Task<List<User>> FindAll();
        Task<User?> FindByUserName(string userName);
        Task<User> ChangeBirthDay(ChangeBirthdayRequest request);
        Task<User> AddUser(User user);
        Task<User> ChangePassword(ChangePasswordRequest request);
    }
}
