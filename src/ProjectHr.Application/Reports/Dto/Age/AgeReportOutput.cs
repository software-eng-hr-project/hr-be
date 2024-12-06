using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.Age;

public class AgeReportOutput
{
    public List<AgeReportDto> Data { get; set; }

    public int Under18Count { get; set; }
    public int Between18_25Count { get; set; }
    public int Between26_34Count { get; set; }
    public int Between35_44Count { get; set; }
    public int Between45_54Count { get; set; }
    public int Between55_64Count { get; set; }
    public int Over65Count { get; set; }
    public int TotalCount { get; set; }
}