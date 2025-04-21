using Talekhisi.Models;

namespace Talekhisi.Services
{
    public interface IProfileService
    {
        Task<UpdateUserInfoDto> GetUserProfileAsync(Guid id);
        Task<UpdateUserInfoDto> UpdateUserProfileInfoAsync(Guid id, UpdateUserInfoDto UpdateUserInfoDto);
        Task<bool> UpdateUserProfilePasswordAsync(Guid id, string newPassword , string oldPassword);

    }
}
