using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;
using ThirdProject_BackEnd.Services.RefreshTokenService;
using ThirdProjectTestUnit.Data;

namespace ThirdProjectTestUnit.Services
{
    public class RefreshTokenServiceTests
    {
        private readonly AppDbContextTests _appDbContextTests;

        public RefreshTokenServiceTests()
        {
            _appDbContextTests = new AppDbContextTests();
        }

        [Theory]
        [InlineData("Phuong1")]
        public async void GenerateRefreshToken_ShouldReturnRefreshToken(string userName)
        {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var refreshTokenService = new RefreshTokenService(dbContext);
            
            //act
            var refreshToken = refreshTokenService.GenerateRefreshToken(userName);

            //assert
            refreshToken.Should().NotBeNull();
            refreshToken.Should().BeOfType<RefreshToken>();
        }

        [Theory]
        [InlineData("Phuong1","Phuong1")]
        public async void ValidateRefreshToken_ShouldReturnTrue(string userName, string oldRefreshToken) {
            //arrange
            var dbContext = await _appDbContextTests.GetDatabaseContext();
            var refreshTokenService = new RefreshTokenService(dbContext);

            //act
            var result = await refreshTokenService.ValidateRefreshToken(userName, oldRefreshToken);

            //assert
            result.Should().BeTrue();
        }
    }
}
