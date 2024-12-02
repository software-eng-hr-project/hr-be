using AutoMapper;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;
using ProjectHr.Extensions;

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
    }
}