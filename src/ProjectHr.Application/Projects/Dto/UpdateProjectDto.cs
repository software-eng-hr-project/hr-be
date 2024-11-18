using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Projects.Dto;

public class UpdateProjectDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public CreateProjectManagerDto Manager { get; set; }
}