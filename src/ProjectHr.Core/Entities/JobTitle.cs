using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class JobTitle : FullAuditedEntity
{
    
    public string Name { get; set; }
    
    public ICollection<User> Users {get; set; }
    
    public ICollection<ProjectMember> ProjectMembers {get; set; }
    
}