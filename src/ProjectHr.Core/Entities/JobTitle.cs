using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class JobTitle
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<User> Users {get; set; }
}