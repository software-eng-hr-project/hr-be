﻿using System.Collections.Generic;
using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Entities;

namespace ProjectHr.DataAccess.Dto;

public class DataAccessMapProfile: Profile
{
    public DataAccessMapProfile()
    {
        CreateMap<JobTitle, JobTitleDto>().ReverseMap();
        CreateMap<EmployeeLayoff, EmployeeLayoffDto>().ReverseMap();
    }
}