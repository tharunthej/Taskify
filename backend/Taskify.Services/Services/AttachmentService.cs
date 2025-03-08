using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Models.Models;
using Taskify.Services.Interfaces;

namespace Taskify.Services.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly AppDbContext _context;

        public AttachmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetAllAttachmentsAsync()
        {
            return await _context.Attachments
                .Include(a => a.Uploader)
                .Include(a => a.Task)
                .ToListAsync();
        }

        public async Task<Attachment?> GetAttachmentByIdAsync(int id)
        {
            return await _context.Attachments
                .Include(a => a.Uploader)
                .Include(a => a.Task)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attachment> CreateAttachmentAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task UpdateAttachmentAsync(Attachment attachment)
        {
            _context.Entry(attachment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttachmentAsync(int id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment != null)
            {
                _context.Attachments.Remove(attachment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
