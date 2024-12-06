using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHr.Authorization;
using ProjectHr.Authorization.Users;
using ProjectHr.Entities;
using ProjectHr.Enums;
using ProjectHr.Extensions;
using ProjectHr.Reports.Dto;
using ProjectHr.Reports.Dto.Age;
using ProjectHr.Reports.Dto.Blood;
using ProjectHr.Reports.Dto.DisabilityLevel;
using ProjectHr.Reports.Dto.Education;
using ProjectHr.Reports.Dto.JobTitle;
using ProjectHr.Reports.Dto.MarriedStatus;

namespace ProjectHr.Reports;

[AbpAuthorize]
[Route("/api/report")]
public class ReportAppService : ProjectHrAppServiceBase
{
    private readonly IRepository<User, long> _userRepository;
    private readonly IRepository<JobTitle, int> _jobTitleRepository;

    public ReportAppService(
        IRepository<User, long> userRepository,
        IRepository<JobTitle, int> jobTitleRepository
    )
    {
        _userRepository = userRepository;
        _jobTitleRepository = jobTitleRepository;
    }
    [AbpAuthorize(PermissionNames.Page_Report)]
    [HttpPost]
    public async Task<dynamic> GetReports([FromQuery] ReportParams reportParams, [FromBody] ReportInput input)
    {
        if (reportParams == ReportParams.Gender)
            return GetGenderReport(input);
        if (reportParams == ReportParams.Military)
            return GetMilitaryReport(input);
        if (reportParams == ReportParams.Education)
            return GetEducationReport(input);
        if (reportParams == ReportParams.EmploymentType)
            return GetEmploymentTypeReport(input);
        if (reportParams == ReportParams.BloodType)
            return GetBloodTypeReport(input);
        if (reportParams == ReportParams.Disability)
            return GetDisabilityLevelReport(input);
        if (reportParams == ReportParams.MarriedStatus)
            return GetMarriedStatusReport(input);
        if (reportParams == ReportParams.Age)
            return GetAgeReport(input);
        if (reportParams == ReportParams.JobTitle)
            return GetJobTitleReport(input);
        // if (reportParams == ReportParams.JobTitle)
        //     return GetJobTitleReport(input);
        

        throw new UserFriendlyException("Invalid report type");
    }
    private GenderReportOutput GetGenderReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);

        var totalCount = users.Count();
        var genderCounts = Enum.GetValues(typeof(Gender))
            .Cast<Gender>()
            .ToDictionary(type => type, type => users.Count(x => x.Gender == type));
        
        var userDto = ObjectMapper.Map<List<GenderReportDto>>(users);
        
        var genderReportOutput = new GenderReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(genderReportOutput, genderCounts);
        return genderReportOutput;
    }
    private  MilitaryReportOutput GetMilitaryReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);

        var totalCount = users.Count();
        var militaryStatusCounts = Enum.GetValues(typeof(MilitaryStatus))
            .Cast<MilitaryStatus>()
            .ToDictionary(type => type, type => users.Count(x => x.MilitaryStatus == type));

        var userDto = ObjectMapper.Map<List<MilitaryReportDto>>(users);
        var militaryReportOutput = new MilitaryReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(militaryReportOutput, militaryStatusCounts);
        return militaryReportOutput;
    }
    private  EducationReportOutput GetEducationReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);

        var totalCount = users.Count();
        var educationStatusCounts = Enum.GetValues(typeof(EducationStatus))
            .Cast<EducationStatus>()
            .ToDictionary(type => type, type => users.Count(x => x.HigherEducationStatus == type));

        var userDto = ObjectMapper.Map<List<EducationReportDto>>(users);
        var educationReportOutput = new EducationReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(educationReportOutput, educationStatusCounts);
        return educationReportOutput;
    }
    private EmploymentTypeReportOutput GetEmploymentTypeReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);

        var totalCount = users.Count();
        var employmentTypeCounts = Enum.GetValues(typeof(EmploymentType))
            .Cast<EmploymentType>()
            .ToDictionary(type => type, type => users.Count(x => x.EmploymentType == type));

        var userDto = ObjectMapper.Map<List<EmploymentTypeReportDto>>(users);
        var employmentReportOutput = new EmploymentTypeReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(employmentReportOutput, employmentTypeCounts);

        return employmentReportOutput;
    }
    
    private BloodTypeReportOutput GetBloodTypeReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);
    
        var totalCount = users.Count();
        var bloodTypeCounts = Enum.GetValues(typeof(BloodType))
            .Cast<BloodType>()
            .ToDictionary(type => type, type => users.Count(x => x.BloodType == type));
    
    
        var userDto = ObjectMapper.Map<List<BloodTypeReportDto>>(users);
        var bloodTypeReportOutput = new BloodTypeReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(bloodTypeReportOutput, bloodTypeCounts);
    
        return bloodTypeReportOutput;
    }
    private DisabilityLevelReportOutput GetDisabilityLevelReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);
    
        var totalCount = users.Count();
        var disabilityLevelCounts = Enum.GetValues(typeof(DisabilityLevel))
            .Cast<DisabilityLevel>()
            .ToDictionary(type => type, type => users.Count(x => x.DisabilityLevel == type));
    
    
        var userDto = ObjectMapper.Map<List<DisabilityLevelReportDto>>(users);
        var disabilityLevelReportOutput = new DisabilityLevelReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(disabilityLevelReportOutput, disabilityLevelCounts);
    
        return disabilityLevelReportOutput;
    }
    private MarriedStatusReportOutput GetMarriedStatusReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);
    
        var totalCount = users.Count();
        var marriedStatusCounts = Enum.GetValues(typeof(MarriedStatus))
            .Cast<MarriedStatus>()
            .ToDictionary(type => type, type => users.Count(x => x.MarriedStatus == type));
        
        var userDto = ObjectMapper.Map<List<MarriedStatusReportDto>>(users);
        var  marriedStatusReportOutput = new MarriedStatusReportOutput()
        {
            Data = userDto, 
            TotalCount = totalCount
        };
        SetCounts(marriedStatusReportOutput, marriedStatusCounts);
    
        return marriedStatusReportOutput;
    }
    private AgeReportOutput GetAgeReport(ReportInput input)
    {
        var users = GetUserWithFilter(input).ToList();
            
        // new int[] = {18, 25, 34  }
        var totalCount = users.Count();
        var usersWithBirthday = users.Where(user => user.Birthday.Year > 1900);
        var under18Count =  usersWithBirthday.Count(user => DateTime.Now.Year - user.Birthday.Year < 18 );
        var between18_25Count =  usersWithBirthday.Count(user => DateTime.Now.Year - user.Birthday.Year >= 18 &&  DateTime.Now.Year - user.Birthday.Year <= 25);

        return null;
    }

    private JobTitleReportOutput GetJobTitleReport(ReportInput input)
    {
        var users = GetUserWithFilter(input).Include(u => u.JobTitle);
        var totalCount = users.Count();
    
        var jobTitles = _jobTitleRepository.GetAllList(); 
        var jobTitleCounts = new Dictionary<int, int>();
    
        foreach (var jobTitle in jobTitles)
        {
            var count = users.Count(u => u.JobTitleId == jobTitle.Id);
            jobTitleCounts[jobTitle.Id] = count;
        }
    
        var userDto = ObjectMapper.Map<List<JobTitleReportDto>>(users);
        var jobTitleReportOutput = new JobTitleReportOutput()
        {
            Data = userDto,
            TotalCount = totalCount
        };
    
        SetJobTitleCounts(jobTitleReportOutput, jobTitleCounts);
    
        return jobTitleReportOutput;
    }
    
    private void SetJobTitleCounts(JobTitleReportOutput reportOutput, Dictionary<int, int> counts)
    {
        var jobTitles = _jobTitleRepository.GetAll();
    
        foreach (var jobTitle in jobTitles)
        {
            var propertyName = jobTitle.Name.Replace(" ", "") + "Count";
            var count = counts.GetValueOrDefault(jobTitle.Id);
        
            var propertyInfo = reportOutput.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(reportOutput, count);
            }
        }
    }
    

    private void SetCounts<TEnum>(object reportOutput, Dictionary<TEnum, int> counts)
        where TEnum : Enum
    {
        foreach (var enumValue in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
        {
            var propertyName = enumValue.ToString() + "Count";
            var count = counts.GetValueOrDefault(enumValue);

            var propertyInfo = reportOutput.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(reportOutput, count);
            }
        }
    }
    private IQueryable<User>GetUserWithFilter(ReportInput input)
    {
        var users = _userRepository.GetAll()
            .Include(x => x.ProjectMembers)
            .WhereIf(input.ProjectId != 0, user =>
                user.ProjectMembers.Any(member => member.ProjectId == input.ProjectId))
            .WhereIf(input.City != null, user => user.City == input.City)
            .WhereIf(input.Year != null, user => user.CreationTime.Year.ToString() == input.Year)
            .WhereIf(input.Year == null && input.Month != null, user => user.CreationTime.Month.ToString() == input.Month);
        return users;
    }
}