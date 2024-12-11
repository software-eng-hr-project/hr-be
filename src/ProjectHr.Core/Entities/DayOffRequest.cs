using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectHr.Authorization.Users;
using ProjectHr.Extensions;
using User = Bugsnag.Payload.User;

namespace ProjectHr.Entities;

public class DayOffRequest: FullAuditedEntity
{
    public string UserId { get; set; }
    public int DayOffTypeId { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int UsedDays { get; set; }
    public DayOffRequestStatus Status { get; set; }
    
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
    
    [ForeignKey(nameof(DayOffTypeId))]
    public DayOffType DayOffType { get; set; }
    
}

public enum DayOffRequestStatus
{
    [AlternateValue("Onay Aşamasında")]
    Pending = 1,
    [AlternateValue("Kabul Edildi")]
    Approved,
    [AlternateValue("Reddedildi")]
    Rejected,
    [AlternateValue("İptal Edildi")]
    Cancelled
}