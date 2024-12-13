using JetBrains.Annotations;

namespace ProjectHr.Reports.Dto.Age;

public class AgeReportDto
{
    public string FullName { get; set; }
    
    [CanBeNull] public string Age { get; set; }
    
    public bool IsActive { get; set; }
}