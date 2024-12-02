using JetBrains.Annotations;
using ProjectHr.DataAccess.Dto;
using ProjectHr.Users.Dto;

namespace ProjectHr.ProjectMembers.Dto;

public class ProjectMemberWithUserDto
{
    public JobTitleDto JobTitle { get; set; }
    
    public ProjectUserDto User { get; set; }
    
    public int ProjectId { get; set; }
    
    [CanBeNull] public string TeamName { get; set; }

    public bool IsManager { get; set; }
    
    public bool IsContributing { get; set; }
}