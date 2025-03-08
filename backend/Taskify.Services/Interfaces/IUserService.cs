using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int id);

        Task<User> CreateUserAsync(User user);

        Task UpdateUserAsync(User user);
        
        Task DeleteUserAsync(int id);
    }
}
