using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> AuthenticateUser(string email, string password);
        Task<User> RegisterUser(User newUser, string password);
        string GenerateJwtToken(User user);
    }
}