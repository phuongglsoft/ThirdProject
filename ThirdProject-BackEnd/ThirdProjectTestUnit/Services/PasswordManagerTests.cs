using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ThirdProject_BackEnd.Services.PasswordManager;

namespace ThirdProjectTestUnit.Services
{
    public class PasswordManagerTests
    {
        [Fact]
        public void CreatePasswordHash_ShouldCreateHashAndSalt()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            var passwordManager = new PasswordManager(configuration);
            byte[] passwordHash, passwordSalt;

            // Act
            passwordManager.CreatePasswordHash("password123", out passwordHash, out passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.NotEmpty(passwordHash);
            Assert.NotEmpty(passwordSalt);
        }
        [Fact]
        public void GenerateJWT_ShouldGenerateValidToken()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["JWT:key"]).Returns("TLUQQ1YCiTX8IFtd5HYQ7QLprv7osc6gxs98Ao3l");
            A.CallTo(() => configuration["Jwt:Issuer"]).Returns("*");
            A.CallTo(() => configuration["Jwt:Audience"]).Returns("*");
            var passwordManager = new PasswordManager(configuration);

            // Act
            var jwtToken = passwordManager.GenerateJWT("testUser");

            // Assert
            Assert.NotNull(jwtToken);
            Assert.NotEmpty(jwtToken);
      
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "*",
                ValidAudience = "*",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TLUQQ1YCiTX8IFtd5HYQ7QLprv7osc6gxs98Ao3l"))
            };

            var principal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out _);
            Assert.NotNull(principal);
        }


        [Fact]
        public void VerifyPassword_ShouldReturnTrueForValidPassword()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            var passwordManager = new PasswordManager(configuration);
            byte[] passwordHash, passwordSalt;
            passwordManager.CreatePasswordHash("password123", out passwordHash, out passwordSalt);

            // Act
            var result = passwordManager.VerifyPassword("password123", passwordHash, passwordSalt);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void VerifyPassword_ShouldReturnFalseForInvalidPassword()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            var passwordManager = new PasswordManager(configuration);
            byte[] passwordHash, passwordSalt;
            passwordManager.CreatePasswordHash("password123", out passwordHash, out passwordSalt);

            // Act
            var result = passwordManager.VerifyPassword("password456", passwordHash, passwordSalt);

            // Assert
            Assert.False(result);
        }
    }
}
