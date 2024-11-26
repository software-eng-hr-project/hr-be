using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using ProjectHr.Entities;
using ProjectHr.DataAccess.Dto;


namespace ProjectHr.DataAccess;

[Route("/api/data-access")]
public class DataAccessAppService: ProjectHrAppServiceBase
{
    private readonly IRepository<JobTitle, int> _jobTitleRepository;
    private readonly IRepository<EmployeeLayoff, int> _employeeLayoffRepository;

    public DataAccessAppService(
        IRepository<JobTitle, int> jobTitleRepository,
        IRepository<EmployeeLayoff, int> employeeLayoffRepository
        )
    {
        _jobTitleRepository = jobTitleRepository;
        _employeeLayoffRepository = employeeLayoffRepository;
    }
    
    [HttpGet("job-title")]
    public async Task<List<JobTitleDto>> GetAllJobTitle()
    {
        var jobTitles = _jobTitleRepository.GetAllList();
        
        var jobTitleDtos = ObjectMapper.Map<List<JobTitleDto>>(jobTitles);
        return jobTitleDtos;
    }
    [HttpGet("employee-layoff")]
    public async Task<List<EmployeeLayoffDto>> GetAllEmployeeLayoff()
    {
        var employeeLayoffs = _employeeLayoffRepository.GetAllList();
        
        var employeeLayoffDtos = ObjectMapper.Map<List<EmployeeLayoffDto>>(employeeLayoffs);
        return employeeLayoffDtos;
    }
}