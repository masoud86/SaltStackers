using System.Security.Claims;

namespace SaltStackers.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();
        
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        bool IsExpiredToken(string token);

        Task<bool> UpdateRefreshTokenAsync(string username, string refreshToken);
    }
}
