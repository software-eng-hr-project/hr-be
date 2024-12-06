using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.Education;

public class EducationReportOutput
{
    public List<EducationReportDto> Data { get; set; }

    public int PrimarySchoolCount { get; set; }
    public int MiddleSchoolCount { get; set; }
    public int HighSchoolCount { get; set; }
    public int AssociateDegreeCount { get; set; }
    public int BachelorsDegreeCount { get; set; }
    public int MasterDegreeCount { get; set; }
    public int PhdDegreeCount { get; set; }
    public int TotalCount { get; set; }
}