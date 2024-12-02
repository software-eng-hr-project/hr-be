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
using ProjectHr.Reports.Dto;

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

        throw new UserFriendlyException("Invalid report type");
    }
    private GenderReportOutput GetGenderReport(ReportInput input)
    {
        var users = _userRepository.GetAll()
            .Include(x => x.ProjectMembers)
            .WhereIf(input.ProjectId != null, user =>
                user.ProjectMembers.Any(member => member.ProjectId == input.ProjectId))
            .WhereIf(input.City != null, user => user.City == input.City)
            .WhereIf(input.Year != null, user => user.CreationTime.Year.ToString() == input.Year)
            .WhereIf(input.Year == null && input.Month != null, user => user.CreationTime.Month.ToString() == input.Month);

        var totalCount = users.Count();
        var maleCount = users.Count(x => x.Gender == Gender.Male);
        var femaleCount = users.Count(x => x.Gender == Gender.Female);
        var otherCount = users.Count(x => x.Gender == Gender.Other);
        
        var userDto = ObjectMapper.Map<List<GenderReportDto>>(users);
        
        var genderReportOutput = new GenderReportOutput
        {
            Data = userDto,
            TotalCount = totalCount,
            MaleCount = maleCount,
            FemaleCount = femaleCount,
            OtherCount = otherCount
        };
        return genderReportOutput;
    }
    private  List<MilitaryReportDto> GetMilitaryReport(ReportInput input)
    {
        var users = _userRepository.GetAll()
            .Include(x => x.ProjectMembers)
            .WhereIf(input.ProjectId != null, user =>
                user.ProjectMembers.Any(member => member.ProjectId == input.ProjectId))
            .WhereIf(input.City != null, user => user.City == input.City)
            .WhereIf(input.Year != null, user => user.CreationTime.Year.ToString() == input.Year)
            .WhereIf(input.Year == null && input.Month != null, user => user.CreationTime.Month.ToString() == input.Month);

        var totalCount = users.Count();
        var DoneCount = users.Count(x => x.MilitaryStatus == MilitaryStatus.Done);
        var NotDoneCount = users.Count(x => x.MilitaryStatus == MilitaryStatus.NotDone);
        var ExemptCount = users.Count(x => x.MilitaryStatus == MilitaryStatus.Exempt);

        var userDto = ObjectMapper.Map<List<MilitaryReportDto>>(users);
        return userDto;
    }
    private  List<MilitaryReportDto> GetEducationReport(ReportInput input)
    {

        return null;
    }
}
// var Count = users.Count(x => x.MilitaryStatus == );
// var Count = users.Count(x => x.MilitaryStatus == );
// var Count = users.Count(x => x.MilitaryStatus == );