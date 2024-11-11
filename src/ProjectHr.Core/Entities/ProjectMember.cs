using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using ProjectHr.Authorization.Users;

namespace ProjectHr.Entities;

public class ProjectMember: FullAuditedEntity
{
    public int JobTitleId { get; set; }
    public long UserId { get; set; }
    public int ProjectId { get; set; }
    
    [ForeignKey(nameof(JobTitleId))]
    public JobTitle JobTitle { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; }
    
    
}