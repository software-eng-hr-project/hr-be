using JetBrains.Annotations;

namespace ProjectHr.ProjectMembers.Dto;

public class CreateProjectMemberDto
{
    
    public long UserId { get; set; }
    
    public int JobTitleId { get; set; }
    
     [CanBeNull] public string TeamName { get; set; }

    public bool IsManager { get; set; } = false;

}