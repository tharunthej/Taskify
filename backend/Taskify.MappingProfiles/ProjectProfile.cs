using AutoMapper;
using Taskify.DTO.ProjectsDTO;
using Taskify.DTO.TasksDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members!.Count))
                .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks!.Count))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

            CreateMap<TaskItem, TaskResponseDto>()
                .ForMember(dest => dest.StatusId, 
                    opt => opt.MapFrom(src => src.StatusId!));

            CreateMap<UpdateProjectDto, Project>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}