﻿using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using ProjectHr.Constants;

namespace ProjectHr.Users.Dto;

public class EmployeeLayoffInfoDto
{
    public DateTime DismissalDate { get; set; }
    public int EmployeeLayoffId { get; set; }
    
    [StringLength(LengthConstants.MaxLayoffReason)]
    [CanBeNull] public string LayoffReason { get; set; }
}