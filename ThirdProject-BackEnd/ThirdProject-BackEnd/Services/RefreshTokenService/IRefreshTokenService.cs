using ThirdProject_BackEnd.Models;

namespace ThirdProject_BackEnd.Services.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken?> GetRefreshTokenByToken(string token);
        Task<RefreshToken?> GetRefreshTokenByUserName(string userName);
        Task<RefreshToken> AddRefreshToken(RefreshToken request);
        Task<RefreshToken> UpdateRefreshToken(RefreshToken request);
        RefreshToken GenerateRefreshToken(string userName);
        void SetFreshToken(RefreshToken newRefreshToken, HttpResponse response);
        Task<bool> ValidateRefreshToken(string userName, string refreshToken);
    }
}
