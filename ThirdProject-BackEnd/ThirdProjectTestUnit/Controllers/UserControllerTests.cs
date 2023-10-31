using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ThirdProject_BackEnd.Controllers;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.UserService;

namespace ThirdProjectTestUnit.Controllers
{
    public class UserControllerTests
    {
        private readonly IUserService _userService;

        public UserControllerTests()
        {
            _userService = A.Fake<IUserService>();
        }
        [Fact]
        public async Task ChangePassword_ShouldReturnOK()
        {
            //arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUser") // Simulating user claims
            };
            var user = new ClaimsIdentity(claims, "TestAuthentication");
            var controller = new UserController(_userService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(user) }
                }
            };
            var request = new ChangePasswordRequest
            {
                user_name = "TestUser",
                new_password = "NewPassword"
            };

            //act
            var result = await controller.ChangePassword(request);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChangePassword_ShouldReturnUnauthorized()
        {
            //arrange
            var nullUserClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUser")
            };
            var nullUserIdentity = new ClaimsIdentity(nullUserClaims, "TestAuthentication");
            var controller = new UserController(_userService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(nullUserIdentity)
                    }
                }
            };
            var request = new ChangePasswordRequest
            {
                user_name = "TestUser",
                new_password = "NewPassword"
            };
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            //act
            var result = await controller.ChangePassword(request);

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task ChangeBirthday_ShouldReturnNoContent() {
            //arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUser") // Simulating user claims
            };
            var user = new ClaimsIdentity(claims, "TestAuthentication");
            var controller = new UserController(_userService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(user) }
                }
            };

            var request = new ChangeBirthdayRequest{new_birth_day = DateTime.UtcNow.AddYears(-15),user_name= "TestUser" };

            A.CallTo(() => _userService.ChangeBirthDay(request));

            //act
            var result = await controller.ChangeBirthDay(request);

            //assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ChangeBirthday_ShouldReturnUnauthorized() {
            //arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "Phuong") // Simulating user claims
            };
            var user = new ClaimsIdentity(claims, "TestAuthentication");
            var controller = new UserController(_userService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(user) }
                }
            };

            var request = new ChangeBirthdayRequest { new_birth_day = DateTime.UtcNow.AddYears(-15), user_name = "TestUser" };

            A.CallTo(() => _userService.ChangeBirthDay(request));

            //act
            var result = await controller.ChangeBirthDay(request);

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
