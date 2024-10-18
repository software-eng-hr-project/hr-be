using AutoMapper;
using ProjectHr.Entities;

namespace ProjectHr.JobTitles.Dto;

public class JobTitleMapProfile: Profile
{
    public JobTitleMapProfile()
    {
        CreateMap<JobTitle, JobTitleDto>().ReverseMap();
    }
}