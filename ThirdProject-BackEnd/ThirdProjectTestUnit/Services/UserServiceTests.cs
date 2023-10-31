using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.PasswordManager;
using ThirdProject_BackEnd.Services.UserService;
using ThirdProjectTestUnit.Data;

namespace ThirdProjectTestUnit.Services
{

    public class UserServiceTests
    {
        private readonly AppDbContextTests _appDbContextTests = new AppDbContextTests();

        [Fact]
        public async void ChangePassword_ReturnUser()
        {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var passwordManager = A.Fake<IPasswordManager>();
            var userService = new UserService(dbContext, passwordManager);
            var request = new ChangePasswordRequest{new_password="1234",user_name="Phuong1" };

            //act
            var user = await userService.ChangePassword(request);
            
            //assert
            user.Should().NotBeNull();
            user.Should().BeOfType<User>();
        }

        [Fact]
        public async void ChangePassword_ShouldThrowException()
        {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var passwordManager = A.Fake<IPasswordManager>();
            var userService = new UserService(dbContext, passwordManager);
            var request = new ChangePasswordRequest { new_password = "1234", user_name = "Phuong100" };

            //act


            //assert
            var ex = await Assert.ThrowsAsync<Exception>(async () => await userService.ChangePassword(request));
        }

        [Fact]
        public async void ChangeBirthDay_ReturnUser()
        {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var passwordManager = A.Fake<IPasswordManager>();
            var userService = new UserService(dbContext, passwordManager);
            var request = new ChangeBirthdayRequest {user_name="Phuong1",new_birth_day=new DateTime(2019,3,3) };

            //act
            var user =await userService.ChangeBirthDay(request);

            //assert
            Assert.True(user.birth_day.Month == 3);
            Assert.True(user.birth_day.Year == 2019);
            Assert.True(user.birth_day.Day == 3);
        }

        [Fact]
        public async void ChangeBirthDay_ShouldThrowException()
        {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var passwordManager = A.Fake<IPasswordManager>();
            var userService = new UserService(dbContext, passwordManager);
            var request = new ChangeBirthdayRequest { user_name = "Phuong100", new_birth_day = new DateTime(2019, 3, 3) };

            //act


            //assert
            await Assert.ThrowsAsync<Exception>(async () => await userService.ChangeBirthDay(request));
        }
    }
    
}
