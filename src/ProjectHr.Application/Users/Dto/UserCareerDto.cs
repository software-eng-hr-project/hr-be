using System.Collections.Generic;
using Abp.Application.Services.Dto;
using ProjectHr.ProjectMembers.Dto;

namespace ProjectHr.Users.Dto;

public class UserCareerDto: EntityDto<long>
{
    public ICollection<ProjectMemberWithProject> ProjectMembers { get; set; }
}