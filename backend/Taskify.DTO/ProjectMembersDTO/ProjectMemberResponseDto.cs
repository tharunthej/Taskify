namespace Taskify.DTO.ProjectMembersDTO
{
    public class ProjectMemberResponseDto
    {
        public int Id { get; set; }
        
        public int ProjectId { get; set; }
        
        public int UserId { get; set; }
        
        public int UserRoleId { get; set; }
        
        // Flattened values from navigation properties.
        public string ProjectName { get; set; } = string.Empty;
        
        public string UserName { get; set; } = string.Empty;
    }
}
