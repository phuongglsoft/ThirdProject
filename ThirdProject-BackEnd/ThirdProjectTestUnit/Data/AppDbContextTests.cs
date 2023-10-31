using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;

namespace ThirdProjectTestUnit.Data
{
    public class AppDbContextTests
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<AppDbContext> GetDatabaseContext()
        {
            var configuration = A.Fake<IConfiguration>();
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var databaseContext = new AppDbContext(options, configuration);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Users.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    CreatePasswordHash($"Phuong{i}", out byte[] passwordHash, out byte[] passwordSalt);
                    databaseContext.Users.Add(new User
                    {
                        user_name = $"Phuong{i}",
                        birth_day = DateTime.UtcNow.AddYears(-15),
                        create_time = DateTime.UtcNow,
                        last_login = DateTime.UtcNow,
                        password_hash= passwordHash,
                        password_salt = passwordSalt
                    }); ;
                }
            }
            if (await databaseContext.RefreshTokens.CountAsync() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.RefreshTokens.Add(new RefreshToken
                    {
                      create_time= DateTime.UtcNow,
                      expires_time = DateTime.UtcNow.AddDays(15),
                      token= $"Phuong{i}",
                      user_name = $"Phuong{i}"
                    }); ;
                }
            }
            await databaseContext.SaveChangesAsync();
            return databaseContext;
        }
    }

}
