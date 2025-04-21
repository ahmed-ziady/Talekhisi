using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talekhisi.Data;
using Talekhisi.Entities;
using Talekhisi.Models;

namespace Talekhisi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher;

        public ProfileService(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<UpdateUserInfoDto> GetUserProfileAsync(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return MapToUserDto(user);
        }

        public async Task<UpdateUserInfoDto> UpdateUserProfileInfoAsync(Guid id, UpdateUserInfoDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // Optional: Trim to avoid dirty data
            user.FirstName = userDto.FirstName.Trim();
            user.LastName = userDto.LastName.Trim();
            user.Email = userDto.Email.Trim();
            user.Department = userDto.Department.Trim();
            user.Grade = userDto.Grade.Trim();

            await _context.SaveChangesAsync();

            return MapToUserDto(user);
        }

        public async Task<bool> UpdateUserProfilePasswordAsync(Guid id, string newPassword, string oldPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new ArgumentException("Old password cannot be empty.", nameof(oldPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password cannot be empty.", nameof(newPassword));

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, oldPassword);
            if (result != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Old password is incorrect.");

            user.PasswordHash = _hasher.HashPassword(user, newPassword);
            await _context.SaveChangesAsync();

            return true;
        }

        private static UpdateUserInfoDto MapToUserDto(User user)
        {
            return new UpdateUserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                University = user.University,
                Faculty = user.Faculty,
                Department = user.Department,
                Grade = user.Grade
            };
        }
    }
}
