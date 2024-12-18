using JetBrains.Annotations;
using ProjectHr.DataAccess.Dto;
using ProjectHr.Projects.Dto;

namespace ProjectHr.ProjectMembers.Dto;

public class ProjectMemberWithProject
{
    public JobTitleDto JobTitle { get; set; }
    
    public long UserId { get; set; }
    
    public ProjectDto Project { get; set; }

    public bool IsManager { get; set; }
    
    public bool IsContributing { get; set; }
}