using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ThirdProject_BackEnd.Data;
using ThirdProject_BackEnd.Models;

namespace ThirdProject_BackEnd.Services.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;

        public RefreshTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> AddRefreshToken(RefreshToken request)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.user_name == request.user_name);
            if (refreshToken is not null)
            {
                 _context.RefreshTokens.Remove(refreshToken);
            }
            await _context.RefreshTokens.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public RefreshToken GenerateRefreshToken(string userName)
        {
            var refreshToken = new RefreshToken { 
            create_time = DateTime.UtcNow,
            expires_time = DateTime.UtcNow.AddDays(7),
            user_name = userName,
            token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };
            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.token == token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByUserName(string userName)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.user_name == userName);
        }

        public void SetFreshToken(RefreshToken newRefreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.expires_time
            };
            response.Cookies.Append("refreshToken", newRefreshToken.token, cookieOptions);
        }

        public async Task<RefreshToken> UpdateRefreshToken(RefreshToken request)
        {
            var refreshToken = await GetRefreshTokenByUserName(request.user_name);
            if (refreshToken is null)
            {
                throw new Exception("Cannot find refresh token");
            }
            refreshToken.create_time = request.create_time;
            refreshToken.expires_time = request.expires_time;
            refreshToken.token = request.token;
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<bool> ValidateRefreshToken(string userName, string oldRefreshToken)
        {
            var refreshToken =await GetRefreshTokenByUserName(userName);
            if(refreshToken is null)
            {
                throw new Exception("Invalid refresh token");
            }
            if(refreshToken.expires_time < DateTime.UtcNow)
            {
                return false;
            }
            return refreshToken.token == oldRefreshToken;
        }
    }
}
