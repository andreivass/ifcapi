using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Dtos;

namespace WebApi.Helpers
{
    /// <summary>
    /// Helper class for JWT operations.
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Create OpenApi security scheme.
        /// </summary>
        /// <returns>OpenApi security scheme.</returns>
        public static OpenApiSecurityScheme CreateSecurityScheme() =>
            new()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
            };

        /// <summary>
        /// Create OpenApi security requirements.
        /// </summary>
        /// <returns>OpenApi security requirements.</returns>
        public static OpenApiSecurityRequirement CreateSecurityRequirements() =>
            new()
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                }
            };

        /// <summary>
        /// Create JWT for auth.
        /// </summary>
        /// <param name="dto">Login dto.</param>
        /// <param name="configuration">Application configuration.</param>
        /// <returns>JTW token as string.</returns>
        public static string BuildToken(LoginDto dto, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, dto.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
