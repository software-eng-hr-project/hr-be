using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.Blood;

public class BloodTypeReportOutput
{
    public List<BloodTypeReportDto> Data { get; set; }
    public int TotalCount { get; set; }
    public int ONegativeCount { get; set; }
    public int OPositiveCount { get; set; }
    public int ANegativeCount { get; set; }
    public int APositiveCount { get; set; }
    public int BNegativeCount { get; set; }
    public int BPositiveCount { get; set; }
    public int ABNegativeCount { get; set; }
    public int ABPositiveCount { get; set; }
}