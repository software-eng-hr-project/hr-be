﻿using JetBrains.Annotations;
using ProjectHr.Users.Dto;

namespace ProjectHr.ProjectMembers.Dto;

public class ProjectMemberDto
{
    public int JobTitleId { get; set; }
    
    public long UserId { get; set; }
    
    public int ProjectId { get; set; }

    public bool IsManager { get; set; }
    
    public bool IsContributing { get; set; }
}