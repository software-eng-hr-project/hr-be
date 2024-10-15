using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class JobTitle : FullAuditedEntity
{
    
    public string Name { get; set; }
    
    public ICollection<User> Users {get; set; }
    
    // [ForeignKey(nameof(CreatorUserId))] 
    // public User Creator { get; set; }    // creator id isteyince creator modeli böyle alınıyor  include diyip kullanman lazım 
}