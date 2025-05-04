using AutoMapper;
using Taskify.Models.Models;
using Taskify.DTO.PriorityDTO;

namespace Taskify.MappingProfiles
{
    public class TaskPriorityProfile : Profile
    {
        public TaskPriorityProfile()
        {
            // Map from Entity to DTO
            CreateMap<TaskPriority, TaskPrioritiesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PriorityLevel, opt => opt.MapFrom(src => src.PriorityLevel));

            // Reverse map for DTO to Entity (if needed)
            CreateMap<TaskPrioritiesDto, TaskPriority>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PriorityLevel, opt => opt.MapFrom(src => src.PriorityLevel));
        }
    }
}