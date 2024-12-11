using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.Birthday;

public class BirthdayReportOutput
{
    public List<BirthdayReportDto> Data { get; set; }

    public int JanuaryCount  { get; set; }
    public int FebruaryCount  { get; set; }
    public int MarchCount  { get; set; }
    public int AprilCount  { get; set; }
    public int MayCount  { get; set; }
    public int JuneCount  { get; set; }
    public int JulyCount { get; set; }
    public int AugustCount  { get; set; }
    public int SeptemberCount { get; set; }
    public int OctoberCount  { get; set; }
    public int NovemberCount  { get; set; }
    public int DecemberCount  { get; set; }
    public int TotalCount { get; set; }
}