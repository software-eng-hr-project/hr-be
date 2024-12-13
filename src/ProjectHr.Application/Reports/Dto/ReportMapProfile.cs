using System;
using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;
using ProjectHr.Extensions;
using ProjectHr.Reports.Dto.Age;
using ProjectHr.Reports.Dto.Blood;
using ProjectHr.Reports.Dto.DisabilityLevel;
using ProjectHr.Reports.Dto.Education;
using ProjectHr.Reports.Dto.JobTitle;
using ProjectHr.Reports.Dto.MarriedStatus;

namespace ProjectHr.Reports.Dto;

public class ReportMapProfile : Profile
{
    public ReportMapProfile()
    {
        CreateMap<User, GenderReportDto>()
            .ForMember(dest => dest.Gender, opt => opt
                .MapFrom(src => src.Gender.GetAlternateValue()));
        CreateMap<User, MilitaryReportDto>()
            .ForMember(dest => dest.MilitaryStatus, opt => opt
                .MapFrom(src => src.MilitaryStatus.GetAlternateValue()));
        CreateMap<User, EducationReportDto>()
            .ForMember(dest => dest.HigherEducationStatus, opt => opt
                .MapFrom(src => src.HigherEducationStatus.GetAlternateValue()));
        CreateMap<User, EmploymentTypeReportDto>()
            .ForMember(dest => dest.EmploymentType, opt => opt
                .MapFrom(src => src.EmploymentType.GetAlternateValue()));
        CreateMap<User, BloodTypeReportDto>()
            .ForMember(dest => dest.BloodType, opt => opt
                .MapFrom(src => src.BloodType.GetAlternateValue()));
        CreateMap<User, DisabilityLevelReportDto>()
            .ForMember(dest => dest.DisabilityLevel, opt => opt
                .MapFrom(src => src.DisabilityLevel.GetAlternateValue()));
        CreateMap<User, MarriedStatusReportDto>()
            .ForMember(dest => dest.MarriedStatus, opt => opt
                .MapFrom(src => src.MarriedStatus.GetAlternateValue()));
        CreateMap<User, JobTitleReportDto>()
            .ForMember(dest => dest.JobTitle, opt => opt
                .MapFrom(src => src.JobTitle.Name));
        CreateMap<User, AgeReportDto>()
            .ForMember(dest => dest.Age, opt => opt
                .MapFrom(src => (DateTime.Now - src.Birthday).TotalDays  / 365.25 ));
    }
}