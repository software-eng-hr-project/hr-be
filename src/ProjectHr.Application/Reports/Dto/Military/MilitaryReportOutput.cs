using System.Collections.Generic;

namespace ProjectHr.Reports.Dto;

public class MilitaryReportOutput
{
    public List<MilitaryReportDto> Data { get; set; }
    public int DoneCount { get; set; }
    public int NotDoneCount { get; set; }
    public int ExemptCount { get; set; }
    public int TotalCount { get; set; }
}