using System.Collections.Generic;

namespace ProjectHr.Reports.Dto;

public class GenderReportOutput
{
    public List<GenderReportDto> Data { get; set; }

    public int MaleCount { get; set; }
    public int FemaleCount { get; set; }
    public int OtherCount { get; set; }
    public int TotalCount { get; set; }
}