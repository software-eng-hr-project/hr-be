using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class EmployeeLayoff: FullAuditedEntity
{
    public string Name { get; set; }
    
    public ICollection<User> Users {get; set; }
}