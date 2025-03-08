using AutoMapper;
using Taskify.DTO.CommentsDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // Map CreateCommentDto to Comment.
            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map Comment to CommentResponseDto, flattening the User.Username property.
            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));

            // Map UpdateCommentDto to Comment. Only update non-null properties.
            CreateMap<UpdateCommentDto, Comment>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
