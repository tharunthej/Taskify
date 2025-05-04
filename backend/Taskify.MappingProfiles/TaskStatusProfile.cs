using AutoMapper;
using Taskify.DTO.StatusDTO;
using TaskStatus = Taskify.Models.Models.TaskStatus;

namespace Taskify.MappingProfiles
{
    public class TaskStatusProfile : Profile
    {
        public TaskStatusProfile()
        {
            // Map from Entity to DTO
            CreateMap<TaskStatus, TaskStatusDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Reverse map for DTO to Entity (if needed)
            CreateMap<TaskStatusDto, TaskStatus>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}