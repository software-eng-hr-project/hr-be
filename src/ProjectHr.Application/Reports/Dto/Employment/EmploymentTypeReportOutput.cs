﻿using System.Collections.Generic;

namespace ProjectHr.Reports.Dto;

public class EmploymentTypeReportOutput
{
    public List<EmploymentTypeReportDto> Data { get; set; }
    public int TotalCount { get; set; }
    public int FullTimeCount { get; set; }
    public int PartTimeCount { get; set; }
}