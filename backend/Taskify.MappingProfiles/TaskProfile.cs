using AutoMapper;
using Taskify.DTO.TasksDTO;
using Taskify.Models.Models;

namespace Taskify.MappingProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            // Map CreateTaskDto to TaskItem
            CreateMap<CreateTaskDto, TaskItem>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map TaskItem to TaskResponseDto, flattening related properties
            CreateMap<TaskItem, TaskResponseDto>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.AssigneeName, 
                           opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Username : string.Empty));

            // Map UpdateTaskDto to TaskItem (only updating non-null values)
            CreateMap<UpdateTaskDto, TaskItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
