using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AICO.Domain.DTOs;
using AICO.Domain.Interfaces.Services;

namespace AICO.Application.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private const string PURPOSE_EMAIL_VERIFICATION = "email_verification";

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string GenerateEmailVerificationToken(Guid userId, string email)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid user ID", nameof(userId));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("purpose", PURPOSE_EMAIL_VERIFICATION)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
            Audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateEmailVerificationToken(string token, out Guid userId, out string email)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));

        userId = Guid.Empty;
        email = string.Empty;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured"));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                ValidateTokenReplay = true
            };

            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            if (jwtToken.Claims.FirstOrDefault(x => x.Type == "purpose")?.Value != PURPOSE_EMAIL_VERIFICATION)
                throw new Exception("Invalid token purpose");

            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var emailClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(emailClaim))
                throw new Exception("Missing required claims");

            if (!Guid.TryParse(userIdClaim, out userId))
                throw new Exception("Invalid user ID format");

            email = emailClaim;
            return true;
        }
        catch (SecurityTokenException ex)
        {
            throw new Exception("Token validation failed", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Token validation failed", ex);
        }
    }

    public TokenResponse GenerateTokens(Guid userId, string email, IEnumerable<string> roles)
    {
        var accessToken = GenerateAccessToken(userId, email, roles);
        var refreshToken = GenerateRefreshToken(userId);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, 
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidateLifetime = false 
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    private string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(Guid userId)
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
