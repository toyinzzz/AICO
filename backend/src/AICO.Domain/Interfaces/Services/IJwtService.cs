using System.Security.Claims;
using AICO.Domain.DTOs;

namespace AICO.Domain.Interfaces.Services;
public interface IJwtService
{
    string GenerateEmailVerificationToken(Guid userId, string email);
    bool ValidateEmailVerificationToken(string token, out Guid userId, out string email);
    TokenResponse GenerateTokens(Guid userId, string email, IEnumerable<string> roles);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
