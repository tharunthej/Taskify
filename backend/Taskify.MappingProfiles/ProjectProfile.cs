using AutoMapper;
using Taskify.DTO.ProjectsDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Map CreateProjectDto to Project and set CreatedAt to current time.
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map Project to ProjectResponseDto, flattening Creator.Username to CreatorName.
            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username));

            // Map UpdateProjectDto to Project, only updating non-null fields.
            CreateMap<UpdateProjectDto, Project>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
