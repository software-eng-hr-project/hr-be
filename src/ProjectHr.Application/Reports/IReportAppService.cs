using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectHr.Authorization.Users;
using ProjectHr.Enums;
using ProjectHr.Reports.Dto;

namespace ProjectHr.Reports;

public interface IReportAppService : IApplicationService
{
    Task<dynamic> GetReports([FromQuery] ReportParams reportParams, [FromBody] ReportInput input);
    IQueryable<User> GetUserWithFilter(ReportInput input);
}