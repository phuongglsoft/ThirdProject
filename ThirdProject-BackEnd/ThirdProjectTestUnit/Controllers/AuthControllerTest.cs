using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdProject_BackEnd.Controllers;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.AuthService;
using ThirdProject_BackEnd.Services.PasswordManager;
using ThirdProject_BackEnd.Services.RefreshTokenService;
using ThirdProject_BackEnd.Services.UserService;

namespace ThirdProjectTestUnit.Controllers
{
    public class AuthControllerTest
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IPasswordManager _passwordManager;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthControllerTest()
        {
            _authService = A.Fake<IAuthService>();
            _userService = A.Fake<IUserService>();
            _passwordManager = A.Fake<IPasswordManager>();
            _refreshTokenService = A.Fake<IRefreshTokenService>();
        }

        [Fact]
        public async Task Login_ReturnOk()
        {
            //Arrange
            var request = new UserLoginRequest() { user_name = "Phuong1", password = "12345" };
            var user = A.Fake<User>();
            var jwt = "adfjhasdiufagbduifgadf";
            var refreshToken = new RefreshToken() { token = "1234", user_name = "Phuong1", create_time = DateTime.Now, expires_time = DateTime.Now.AddDays(15) };
            A.CallTo(() => _refreshTokenService.GenerateRefreshToken(request.user_name)).Returns(refreshToken);
            A.CallTo(() => _authService.ValidateUser(request)).Returns(user);
            var userDTO = new UserDTO { user_name = user.user_name, birth_day = user.birth_day, create_time = user.create_time, last_login = user.last_login };
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);
            AuthResponse authResponse = new AuthResponse() { user = userDTO, jwt = jwt, refresh_token = refreshToken.token };

            //Act
            var result = await controller.Login(request);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<AuthResponse>));
        }
        [Fact]
        public async Task Login_WrongPassword()
        {
            //arrange
            var request = new UserLoginRequest() { user_name = "Phuong1", password = "12345" };
            var user = A.Fake<User>();
            A.CallTo(() => _authService.ValidateUser(request)).Returns(user);
            A.CallTo(() => _passwordManager.VerifyPassword(request.password, default, default)).Returns(false);
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);
            //act
            var result = await controller.Login(request);

            //assert
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Register_ShouldReturnOk()
        {
            //arrange
            var request = new UserRegisterRequest {birth_day = DateTime.UtcNow.AddYears(-20),password = "12341234",user_name="Phuong10" };
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);
            var passwordHash = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; ;
            var passwordSalt = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; ;
            var refreshToken = A.Fake<RefreshToken>();
            var jwt = "fake jwt";
            var user = A.Fake<User>();
            A.CallTo(() => _authService.ValidateUserForRegister(request)).Returns(true);
            A.CallTo(() => _passwordManager.CreatePasswordHash(request.password,out passwordHash, out passwordSalt));
            A.CallTo(() => _userService.AddUser(user));
            A.CallTo(() => _passwordManager.GenerateJWT(user.user_name)).Returns(jwt);
            A.CallTo(() => _refreshTokenService.GenerateRefreshToken(request.user_name)).Returns(refreshToken);
            A.CallTo(() => _refreshTokenService.AddRefreshToken(refreshToken));

            //act
            var result = await controller.Register(request);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<AuthResponse>));
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest()
        {
            //arrange
            var request = new UserRegisterRequest { birth_day = DateTime.UtcNow.AddYears(-20), password = "12341234", user_name = "Phuong10" };
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);
            
            //act
            var result = await controller.Register(request);

            //assert
            result.Should().NotBeNull();
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void RefreshToken_ShouldReturnOk()
        {
            //arrange

            RefreshTokenRequest request = new RefreshTokenRequest{refresh_token="123441",user_name="Phuong1" };
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);

            A.CallTo(() => _refreshTokenService.ValidateRefreshToken(request.user_name, request.refresh_token)).Returns(true);

            //act
            var result = await controller.RefreshToken(request);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(ActionResult<string>));

        }
        [Fact]
        public async void RefreshToken_ShouldReturnUnauthorized()
        {
            //arrange

            RefreshTokenRequest request = new RefreshTokenRequest { refresh_token = "123441", user_name = "Phuong1" };
            var controller = new AuthController(_authService, _userService, _passwordManager, _refreshTokenService);

            A.CallTo(() => _refreshTokenService.ValidateRefreshToken(request.user_name, request.refresh_token)).Returns(false);

            //act
            var result = await controller.RefreshToken(request);

            //assert
            result.Should().NotBeNull();
            Assert.IsType<UnauthorizedObjectResult>(result.Result);

        }
    }
}
