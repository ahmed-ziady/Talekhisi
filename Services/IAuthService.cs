using Talekhisi.Entities;
using Talekhisi.Models;

namespace Talekhisi.Services
{
    public interface IAuthService
    {
        Task<User?> Register(UserDto request);
        Task <String?>Login(UserDto request);
    }
}
