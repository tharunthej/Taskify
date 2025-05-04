using Taskify.DTO.ProjectMembersDTO;
using Taskify.DTO.TasksDTO;

namespace Taskify.DTO.ProjectsDTO
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int CreatorId { get; set; }
        public required string CreatorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MemberCount { get; set; }
        public int TaskCount { get; set; }
        
        // Optional: Include full member/task lists if needed
        public IEnumerable<ProjectMemberResponseDto>? Members { get; set; }
        public IEnumerable<TaskBriefDto>? Tasks { get; set; }
    }

    public class TaskBriefDto 
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int StatusId { get; set; }
        
    }
}
