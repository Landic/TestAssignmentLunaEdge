using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAssignmentLunaEdge.DataAccess.Models;

namespace TestAssignmentLunaEdge.Authentication
{
    public class JWTService : IJWTService
    {
        // IConfiguration object for accessing app configuration values
        private readonly IConfiguration _configuration;

        // Constructor to inject the IConfiguration dependency
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to generate a JWT token for a given user
        public string GenerateJwtToken(User user)
        {
            // Create a symmetric security key using the secret key from the configuration
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
            );

            // Create signing credentials using HMACSHA256 algorithm
            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            // Define claims to be included in the token, such as user ID, email, and username
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            // Create the JWT token, setting its issuer, audience, claims, and expiration time
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // Token expires in 2 hours
                signingCredentials: credentials
            );

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to validate a JWT token and return the claims principal
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Define token validation parameters, such as issuer, audience, and signing key
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Validate the token's issuer
                ValidateAudience = true, // Validate the token's audience
                ValidateLifetime = true, // Validate the token's expiration
                ValidIssuer = _configuration["Jwt:Issuer"], // Issuer to validate
                ValidAudience = _configuration["Jwt:Audience"], // Audience to validate
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])
                ) // The signing key for token verification
            };

            // Validate the token and return the claims principal
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
    }
}
