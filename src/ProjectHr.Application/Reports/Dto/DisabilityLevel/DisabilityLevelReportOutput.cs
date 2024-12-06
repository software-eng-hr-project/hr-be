using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.DisabilityLevel;

public class DisabilityLevelReportOutput
{
    public List<DisabilityLevelReportDto> Data { get; set; }

    public int NoneCount { get; set; }
    public int FirstDegreeCount { get; set; }
    public int SecondDegreeCount { get; set; }
    public int ThirdDegreeCount { get; set; }
    public int TotalCount { get; set; }
}