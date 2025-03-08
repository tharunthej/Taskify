using AutoMapper;
using Taskify.DTO.AttachmentsDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class AttachmentProfile : Profile
    {
        public AttachmentProfile()
        {
            // Map CreateAttachmentDto to Attachment.
            CreateMap<CreateAttachmentDto, Attachment>()
                .ForMember(dest => dest.UploadedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            // Map Attachment to AttachmentResponseDto,
            // flattening Uploader.Username to UploaderName.
            CreateMap<Attachment, AttachmentResponseDto>()
                .ForMember(dest => dest.UploaderName, opt => opt.MapFrom(src => src.Uploader.Username));
            
            // Map UpdateAttachmentDto to Attachment, updating only non-null values.
            CreateMap<UpdateAttachmentDto, Attachment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
