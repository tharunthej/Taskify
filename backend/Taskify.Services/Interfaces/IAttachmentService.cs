using Taskify.Models.Models;

namespace Taskify.Services.Interfaces
{
    public interface IAttachmentService
    {
        Task<IEnumerable<Attachment>> GetAllAttachmentsAsync();

        Task<Attachment?> GetAttachmentByIdAsync(int id);

        Task<Attachment> CreateAttachmentAsync(Attachment attachment);

        Task UpdateAttachmentAsync(Attachment attachment);
        
        Task DeleteAttachmentAsync(int id);
    }
}
