using Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IAuthService
{
    string GenerateToken(Person user);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IConfiguration config, ILogger<AuthService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public string GenerateToken(Person user)
    {
        _logger.LogInformation("Generating JWT token for user: {UserName} (ID: {UserId})", user.UserName, user.Id);
        var secretKey = _config["JwtSettings:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            _logger.LogCritical("JWT SecretKey is missing from configuration! Token generation failed.");
            throw new InvalidOperationException("SecretKey is missing!");
        }
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);
            var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("Successfully generated token for user {UserName}.", user.UserName);
            return generatedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a token for user {UserName}", user.UserName);
            throw;
        }
    }
}