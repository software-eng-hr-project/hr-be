using System;

namespace ProjectHr.Reports.Dto.Birthday;

public class BirthdayReportDto
{
    public string FullName { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public bool IsActive { get; set; }
}