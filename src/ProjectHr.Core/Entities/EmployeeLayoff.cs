using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;

namespace ProjectHr.Entities;

public class EmployeeLayoff: FullAuditedEntity
{
    public string Name { get; set; }
    
    public ICollection<EmployeeLayoffInfo> EmployeeLayoffInfos {get; set; }
}