using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talekhisi.Data;
using Talekhisi.Entities;
using Talekhisi.Models;

namespace Talekhisi.Services
{
    public class AuthService (AppDbContext context,IConfiguration configuration): IAuthService
    {
        public async Task<string?> Login(UserDto request)
        {

            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);


            ArgumentNullException.ThrowIfNull(request);
            if (user is null)
            {
                return null;
            }


            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return createToken(user);
        }

        public async Task<User?> Register(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username)) // Fix logical issue
            {
                return null;
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, request.Password),
                Role = string.IsNullOrWhiteSpace(request.Role) ? "user" : request.Role // Use provided role, else default to "user"
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        private string createToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role) // Ensure role is correctly assigned
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}
