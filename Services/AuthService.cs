using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;   
using Talekhisi.Data;
using Talekhisi.Entities;
using Talekhisi.Models;

namespace Talekhisi.Services
{
    public class AuthService(AppDbContext context, IConfiguration config) : IAuthService
    { 
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;
        private readonly PasswordHasher<User> _hasher = new();

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return null;

            var user = new User
            {

                Username = request.Username.Trim(),
                PasswordHash = _hasher.HashPassword(null!, request.Password),
                Role = string.IsNullOrWhiteSpace(request.Role) ? "user" : request.Role.Trim().ToLower(),
                FirstName = request.FirstName.Trim().ToLower(),
                LastName = request.LastName.Trim().ToLower(),
                Email = request.Email.Trim().ToLower(),
                University = request.University.Trim().ToLower(),
                Faculty = request.Faculty.Trim().ToLower(),
                Department = request.Department.Trim().ToLower(),
                Grade = request.Grade.Trim().ToLower()

            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return null;   

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
                return null;

            return await CreateTokenResponseAsync(user);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            if (request.UserId == Guid.Empty || string.IsNullOrWhiteSpace(request.RefreshToken))
                return null;

            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            return await CreateTokenResponseAsync(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponseAsync(User user) => new()
        {
            AccessToken = GenerateJwtToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user),
            Id = user.Id
        };

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["AppSettings:Token"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config["AppSettings:Issuer"],
                audience: _config["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
