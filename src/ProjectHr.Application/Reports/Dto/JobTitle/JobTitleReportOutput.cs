using System.Collections.Generic;

namespace ProjectHr.Reports.Dto.JobTitle;

public class JobTitleReportOutput
{
    public List<JobTitleReportDto> Data { get; set; }
    
    public int TotalCount { get; set; }
    public int HumanResourcesSpecialistCount { get; set; }
    public int FrontendDevCount { get; set; }
    public int BackendDevCount { get; set; }
    public int FullStackDevCount { get; set; }
    public int MobileDevCount { get; set; }
    public int ProjectManagerCount { get; set; }
    public int BusinessAnalystCount { get; set; }
    public int TeamLeadCount { get; set; }
    public int TechLeadCount { get; set; }
    public int SoftwareTestSpecialistCount { get; set; }
    public int UIUXDesignerCount { get; set; }
    public int DevOpsEngineerCount { get; set; }
    public int EngineeringManagerCount { get; set; }
    public int ProductManagerCount { get; set; }
}