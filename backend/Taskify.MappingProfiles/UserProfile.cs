using AutoMapper;
using Taskify.DTO.UsersDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Create mapping from CreateUserDto to User.
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map User to UserResponseDto.
            CreateMap<User, UserResponseDto>();

            // Map UpdateUserDto to User; only update non-null values.
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
