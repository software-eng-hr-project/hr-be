using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.MarriedStatus;

public class MarriedStatusReportOutput
{
    public List<MarriedStatusReportDto> Data { get; set; }

    public int SingleCount { get; set; }
    public int MarriedCount { get; set; }
    public int DivorcedCount { get; set; }
    public int TotalCount { get; set; }
}