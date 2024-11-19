using JetBrains.Annotations;

namespace ProjectHr.ProjectMembers.Dto;

public class ProjectMemberDto
{
    public int JobTitleId { get; set; }
    
    public long UserId { get; set; }
    
    public int ProjectId { get; set; }
    
    [CanBeNull] public string TeamName { get; set; }

    public bool IsManager { get; set; } = false;
}