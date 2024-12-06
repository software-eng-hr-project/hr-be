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
using ProjectHr.Enums;
using ProjectHr.Extensions;
using ProjectHr.Reports.Dto;
using ProjectHr.Reports.Dto.Blood;
using ProjectHr.Reports.Dto.Education;

namespace ProjectHr.Reports;

[AbpAuthorize]
[Route("/api/report")]
public class ReportAppService : ProjectHrAppServiceBase
{
    private readonly IRepository<User, long> _userRepository;

    public ReportAppService(IRepository<User, long> userRepository)
    {
        _userRepository = userRepository;
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
        
        var genderReportOutput = new GenderReportOutput
        {
            Data = userDto,
            TotalCount = totalCount,
            MaleCount = genderCounts.GetValueOrDefault(Gender.Male),
            FemaleCount = genderCounts.GetValueOrDefault(Gender.Female),
            OtherCount = genderCounts.GetValueOrDefault(Gender.Other)
        };
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
            TotalCount = totalCount,
            DoneCount = militaryStatusCounts.GetValueOrDefault(MilitaryStatus.Done),
            NotDoneCount= militaryStatusCounts.GetValueOrDefault(MilitaryStatus.NotDone),
            ExemptCount = militaryStatusCounts.GetValueOrDefault(MilitaryStatus.Exempt)
        };
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
        var militaryReportOutput = new EducationReportOutput()
        {
            Data = userDto,
            TotalCount = totalCount,
            PrimarySchoolCount = educationStatusCounts.GetValueOrDefault(EducationStatus.PrimarySchool),
            MiddleSchoolCount= educationStatusCounts.GetValueOrDefault(EducationStatus.MiddleSchool),
            HighSchoolCount= educationStatusCounts.GetValueOrDefault(EducationStatus.HighSchool),
            AssociateDegreeCount = educationStatusCounts.GetValueOrDefault(EducationStatus.AssociateDegree),
            BachelorsDegreeCount = educationStatusCounts.GetValueOrDefault(EducationStatus.BachelorsDegree),
            MasterDegreeCount = educationStatusCounts.GetValueOrDefault(EducationStatus.MasterDegree),
            PhdDegreeCount = educationStatusCounts.GetValueOrDefault(EducationStatus.PhdDegree)
        };
        return militaryReportOutput;
    }
    private EmploymentTypeReportOutput GetEmploymentTypeReport(ReportInput input)
    {
        var users = GetUserWithFilter(input);

        var totalCount = users.Count();
        var employmentTypeCounts = Enum.GetValues(typeof(EmploymentType))
            .Cast<EmploymentType>()
            .ToDictionary(type => type, type => users.Count(x => x.EmploymentType == type));

        var userDto = ObjectMapper.Map<List<EmploymentTypeReportDto>>(users);
        var employmentReportOutput = new EmploymentTypeReportOutput
        {
            Data = userDto,
            TotalCount = totalCount,
            FullTimeCount = employmentTypeCounts.GetValueOrDefault(EmploymentType.FullTime),
            PartTimeCount = employmentTypeCounts.GetValueOrDefault(EmploymentType.PartTime)
        };

        return employmentReportOutput;
    }
    
    // public BloodTypeReportOutput GetBloodTypeReport(ReportInput input)
    // {
    //     var users = GetUserWithFilter(input);
    //
    //     var totalCount = users.Count();
    //     var bloodTypeCounts = Enum.GetValues(typeof(BloodType))
    //         .Cast<BloodType>()
    //         .ToDictionary(type => type, type => users.Count(x => x.BloodType == type));
    //
    //
    //     var userDto = ObjectMapper.Map<List<BloodTypeReportDto>>(users);
    //     var bloodTypeReportOutput = new BloodTypeReportOutput
    //     {
    //         Data = userDto,
    //         TotalCount = totalCount,
    //         ONegativeCount = bloodTypeCounts.GetValueOrDefault(BloodType.ONegative),
    //         OPositiveCount = bloodTypeCounts.GetValueOrDefault(BloodType.OPositive),
    //         ANegativeCount = bloodTypeCounts.GetValueOrDefault(BloodType.ANegative),
    //         APositiveCount = bloodTypeCounts.GetValueOrDefault(BloodType.APositive),
    //         BNegativeCount = bloodTypeCounts.GetValueOrDefault(BloodType.BNegative),
    //         BPositiveCount = bloodTypeCounts.GetValueOrDefault(BloodType.BPositive),
    //         ABNegativeCount = bloodTypeCounts.GetValueOrDefault(BloodType.ABNegative),
    //         ABPositiveCount = bloodTypeCounts.GetValueOrDefault(BloodType.ABPositive),
    //     };
    //
    //     return bloodTypeReportOutput;
    // }

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