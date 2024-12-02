using ProjectHr.Enums;

namespace ProjectHr.Reports.Dto;

public class MilitaryReportDto
{
    public string FullName { get; set; }
    
    public string MilitaryStatus  { get; set; }
    
    public bool IsActive { get; set; }
}