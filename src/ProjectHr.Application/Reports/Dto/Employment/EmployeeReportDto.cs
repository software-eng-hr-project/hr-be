using System;
using ProjectHr.DataAccess.Dto;

namespace ProjectHr.Reports.Dto;

public class EmployeeReportDto
{
    public string FullName { get; set; }

    public JobTitleDto JobTitle { get; set; }

    public DateTime JobStartDate { get; set; }
    public string EmploymentType { get; set; }
    
    public bool IsActive { get; set; }
}