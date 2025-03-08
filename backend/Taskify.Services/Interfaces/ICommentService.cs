using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();

        Task<Comment?> GetCommentByIdAsync(int id);

        Task<Comment> CreateCommentAsync(Comment comment);
        
        Task UpdateCommentAsync(Comment comment);

        Task DeleteCommentAsync(int id);
    }
}
