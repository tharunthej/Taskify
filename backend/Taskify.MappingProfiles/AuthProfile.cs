using AutoMapper;
using Taskify.DTO.AuthDTO;
using Taskify.DTO.UsersDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, LoginResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Token"]));
            
            CreateMap<CreateUserDto, User>();
        }
    }
}