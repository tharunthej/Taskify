using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int id);

        Task UpdateUserAsync(User user);
        
        Task DeleteUserAsync(int id);

        bool VerifyPassword(User user, string password);

        string HashPassword(string password);
    }
}
