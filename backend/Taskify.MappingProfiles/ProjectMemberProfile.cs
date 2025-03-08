using AutoMapper;
using Taskify.DTO.ProjectMembersDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class ProjectMemberProfile : Profile
    {
        public ProjectMemberProfile()
        {
            // Map from CreateProjectMemberDto to ProjectMember
            CreateMap<CreateProjectMemberDto, ProjectMember>();

            // Map from ProjectMember to ProjectMemberResponseDto.
            // Flatten the Project and User navigation properties to ProjectName and UserName.
            CreateMap<ProjectMember, ProjectMemberResponseDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
        }
    }
}
