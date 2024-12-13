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
using ProjectHr.Reports.Dto.Birthday;
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
        switch (reportParams)
        {
            case ReportParams.Gender:
                return GetGenderReport(input);
            case ReportParams.Military:
                return GetMilitaryReport(input);
            case ReportParams.Education:
                return GetEducationReport(input);
            case ReportParams.Employmee:
                return GetEmploymeeReport(input);
            case ReportParams.BloodType:
                return GetBloodTypeReport(input);
            case ReportParams.Disability:
                return GetDisabilityLevelReport(input);
            case ReportParams.MarriedStatus:
                return GetMarriedStatusReport(input);
            case ReportParams.Age:
                return GetAgeReport(input);
            case ReportParams.Birthday:
                return GetBirthdayReport(input);
            case ReportParams.JobTitle:
                return GetJobTitleReport(input);
            default:
                throw new ArgumentException("Geçersiz rapor parametresi", nameof(reportParams));
        }
        

        throw new UserFriendlyException("Geçersiz rapor tipi");
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
    private EmployeeReportOutput GetEmploymeeReport(ReportInput input)
    {
        var users = GetUserWithFilter(input).Include(x => x.JobTitle);

        var totalCount = users.Count();
        var employmentTypeCounts = Enum.GetValues(typeof(EmploymentType))
            .Cast<EmploymentType>()
            .ToDictionary(type => type, type => users.Count(x => x.EmploymentType == type));

        var userDto = ObjectMapper.Map<List<EmployeeReportDto>>(users);
        var employmentReportOutput = new EmployeeReportOutput()
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
        
        var totalCount = users.Count();
        var usersWithBirthday = users.Where(user => user.Birthday.Year > 1900);
        
        var ageRanges = new List<(string RangeName, int MinAge, int MaxAge)>
        {
            ("Under18", 0, 18),
            ("Between18_25", 18, 26),
            ("Between26_34", 26, 35),
            ("Between35_44", 35, 45),
            ("Between45_54", 45, 55),
            ("Between55_64", 55, 65),
            ("Over65", 65, int.MaxValue)
        };

        var ageCounts = new Dictionary<string, int>();

        foreach (var range in ageRanges)
        {
            // var count = usersWithBirthday.Count(user =>
            //     user.Birthday.Year <= DateTime.Now.Year - range.MinAge &&
            //     user.Birthday.Year >= DateTime.Now.Year - range.MaxAge);
            var count = usersWithBirthday.Count(user =>
                (DateTime.Now - user.Birthday).TotalDays  / 365.25 >= range.MinAge &&
                (DateTime.Now - user.Birthday).TotalDays / 365.25 < range.MaxAge);

            ageCounts[range.RangeName] = count;
        }
        var userDto = ObjectMapper.Map<List<AgeReportDto>>(users);
        var ageReportOutput = new AgeReportOutput
        {
            Data = userDto,
            TotalCount = totalCount
        };
        foreach (var ageCount in ageCounts)
        {
            var propertyName = ageCount.Key + "Count";
            var propertyInfo = ageReportOutput.GetType().GetProperty(propertyName);
        
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(ageReportOutput, ageCount.Value);
            }
        }
        return ageReportOutput;
    }
    private BirthdayReportOutput GetBirthdayReport(ReportInput input)
    {
        var users = GetUserWithFilter(input).ToList();
        
        var totalCount = users.Count();
        var usersWithBirthday = users.Where(user => user.Birthday.Year > 1900);

        var months = new List<(string MonthName, int MonthOrder)>
        {
            ( "January",1 ),
            ( "February",2 ),
            ( "March",3 ),
            ( "April",4 ),
            ( "May",5 ),
            ( "June",6 ),
            ( "July",7 ),
            ( "August",8 ),
            ( "September",9 ),
            ( "October",10 ),
            ( "November",11 ),
            ( "December",12 )
        };

        var monthCounts = new Dictionary<string, int>();

        foreach (var month in months)
        {
            var count = usersWithBirthday.Count(user => user.Birthday.Month == month.MonthOrder);
            monthCounts[month.MonthName] = count;
        }
        var userDto = ObjectMapper.Map<List<BirthdayReportDto>>(users);
        var birthdayReportOutput = new BirthdayReportOutput
        {
            Data = userDto,
            TotalCount = totalCount
        };
        foreach (var monthCount in monthCounts)
        {
            var propertyName = monthCount.Key + "Count";
            var propertyInfo = birthdayReportOutput.GetType().GetProperty(propertyName);
        
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(birthdayReportOutput, monthCount.Value);
            }
        }
        return birthdayReportOutput;
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